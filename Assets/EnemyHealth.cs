using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth, IDataPersistence
{
    public int maxHealth = 100; // Máu tối đa của kẻ thù
    private int currentHealth; // Máu hiện tại của kẻ thù
    private bool isDead = false; // Trạng thái chết của kẻ thù
    [SerializeField] private string id;
    [ContextMenu("Set ID")]
    private void GenerateGuild()
    {
        id = System.Guid.NewGuid().ToString();
    }
    void Start()
    {
        currentHealth = maxHealth; // Khởi tạo máu hiện tại bằng máu tối đa
    }

    // Hàm để kẻ thù nhận sát thương
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Trừ máu hiện tại bằng lượng sát thương nhận được
        if (currentHealth <= 0)
        {
            Die(); // Gọi hàm Die nếu máu hiện tại bằng hoặc nhỏ hơn 0
        }
    }

    // Hàm xử lý khi kẻ thù chết
    void Die()
    {
        // Thêm logic xử lý khi kẻ thù chết, ví dụ như phát hoạt ảnh chết, vô hiệu hóa kẻ thù, v.v.
        Debug.Log("Enemy died!");
        isDead = true; // Đặt trạng thái chết thành true
        transform.gameObject.SetActive(false); // Vô hiệu hóa kẻ thù
    }
    public void SaveData(ref GameData data)
    {
        if (data.enemy.ContainsKey(id))
        {
            data.enemy[id].maxHealth = maxHealth;
            data.enemy[id].currentHealth = currentHealth;
            data.enemy[id].enemyPosition = transform.position;
            data.enemy[id].isDead = isDead;
        }
        else
        {
            data.enemy.Add(id, new EnemyData(maxHealth, currentHealth, transform.position, isDead));
        }
    }
    public void LoadData(GameData data)
    {
        if (data.enemy.ContainsKey(id))
        {
            maxHealth = (int)data.enemy[id].maxHealth;
            currentHealth = (int)data.enemy[id].currentHealth;
            transform.position = data.enemy[id].enemyPosition;
            isDead = data.enemy[id].isDead;
        }
        if (isDead)
        {
            transform.gameObject.SetActive(false);
        }
    }
}
