using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public TMP_Text counterText;
    public AudioClip destroySound;
    public GameObject startPanel;
    public GameObject donePanel;
    public BoxCollider playZone;
    public int targetCount = 10;
    public float speed = 5f;
    public float rotationSpeed = 90f;
    public Transform carVisual;
    public Material carMaterial;
    public Texture2D[] carTextures;

    int carTextureIndex;
    Renderer[] carRenderers;
    MaterialPropertyBlock carProperties;
    bool isPlaying;
    int count;
    BoxCollider cubeCollider;
    AudioSource audioSource;

    void Start()
    {
        cubeCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        carRenderers = carVisual != null
            ? carVisual.GetComponentsInChildren<Renderer>(true)
            : new Renderer[0];
        carProperties = new MaterialPropertyBlock();
        foreach (Renderer carRenderer in carRenderers)
        {
            if (carMaterial != null)
                carRenderer.sharedMaterial = carMaterial;
        }
        carTextureIndex = 0;
        ApplyCarTexture();
        count = 0;
        UpdateCounter();
        startPanel.SetActive(true);
        donePanel.SetActive(false);
        isPlaying = false;
        Time.timeScale = 0f;
        KeepInsidePlayZone();
    }

    void Update()
    {
        if (!isPlaying || Time.timeScale == 0f)
            return;

        if (Input.GetKeyDown(KeyCode.H))
            SwitchCarTexture();

        float distance = speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.forward * distance;
        if (Input.GetKey(KeyCode.S))
            transform.position += Vector3.back * distance;
        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * distance;
        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * distance;
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        KeepInsidePlayZone();
    }

    void SwitchCarTexture()
    {
        if (carTextures == null || carTextures.Length == 0)
            return;

        carTextureIndex = (carTextureIndex + 1) % carTextures.Length;
        ApplyCarTexture();
    }

    void ApplyCarTexture()
    {
        if (carTextures == null || carTextures.Length == 0 || carTextures[carTextureIndex] == null)
            return;

        foreach (Renderer carRenderer in carRenderers)
        {
            carRenderer.GetPropertyBlock(carProperties);
            carProperties.SetTexture("_BaseMap", carTextures[carTextureIndex]);
            carProperties.SetTexture("_MainTex", carTextures[carTextureIndex]);
            carRenderer.SetPropertyBlock(carProperties);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPlaying || !other.CompareTag("Collectible") || !other.enabled)
            return;

        // Disable immediately so a pending Destroy cannot count the same pickup twice.
        other.enabled = false;
        Destroy(other.gameObject);
        transform.localScale += Vector3.one * 0.1f;
        KeepInsidePlayZone();
        count++;
        UpdateCounter();

        // Optional audio must not interrupt counting or level completion.
        if (audioSource != null && destroySound != null)
            audioSource.PlayOneShot(destroySound);

        if (count >= targetCount)
        {
            isPlaying = false;
            startPanel.SetActive(false);
            donePanel.SetActive(true);
            donePanel.transform.SetAsLastSibling();
            Time.timeScale = 0f;
        }
    }

    void UpdateCounter()
    {
        if (counterText != null)
            counterText.text = count.ToString();
    }

    void KeepInsidePlayZone()
    {
        if (playZone == null || cubeCollider == null)
            return;

        Bounds zone = playZone.bounds;
        // Account for rotation and growth when keeping the entire box on the platform.
        Vector3 half = cubeCollider.size * 0.5f;
        Vector3 right = transform.TransformVector(Vector3.right * half.x);
        Vector3 up = transform.TransformVector(Vector3.up * half.y);
        Vector3 forward = transform.TransformVector(Vector3.forward * half.z);
        Vector3 extent = new Vector3(
            Mathf.Abs(right.x) + Mathf.Abs(up.x) + Mathf.Abs(forward.x),
            Mathf.Abs(right.y) + Mathf.Abs(up.y) + Mathf.Abs(forward.y),
            Mathf.Abs(right.z) + Mathf.Abs(up.z) + Mathf.Abs(forward.z));
        Vector3 centerOffset = transform.TransformVector(cubeCollider.center);
        Vector3 center = transform.position + centerOffset;
        center.x = Mathf.Clamp(center.x, zone.min.x + extent.x, zone.max.x - extent.x);
        center.z = Mathf.Clamp(center.z, zone.min.z + extent.z, zone.max.z - extent.z);
        center.y = zone.max.y + extent.y;
        transform.position = center - centerOffset;
    }

    public void StartGame()
    {
        donePanel.SetActive(false);
        startPanel.SetActive(false);
        isPlaying = true;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().path);
    }
}
