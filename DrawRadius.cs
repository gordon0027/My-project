using UnityEngine;

public class DrawRadius : MonoBehaviour
{
    public float heightOffset = 0.1f; // Новая переменная для смещения по высоте
    public int segments = 64;
    public LineRenderer lineRenderer;

    public EnemyDamage enemyDamage; // Ссылка на объект EnemyDamage

    void Start()
    {
        // Создаем LineRenderer, если его нет
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.useWorldSpace = false;
            lineRenderer.positionCount = segments + 1;
        }

        // Если есть ссылка на EnemyDamage, используем ее радиус
        if (enemyDamage != null)
        {
            UpdateRadius(enemyDamage.attackRadius);
        }
        else
        {
            Debug.LogWarning("EnemyDamage reference is not set. Using default radius.");
            UpdateRadius(5f); // Установите значение по умолчанию или другое значение, если ссылка не установлена
        }
    }

    

    void Update()
    {
        // В реальном проекте, возможно, стоит обновлять радиус только при изменении attackRadius в EnemyDamage
        // Здесь мы обновляем радиус в каждом кадре в качестве примера.
        if (enemyDamage != null)
        {
            UpdateRadius(enemyDamage.attackRadius);
        }

        // Используем Physics.OverlapSphere для определения объектов в радиусе
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyDamage.attackRadius);
        
        // Проверяем каждый найденный коллайдер
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Если объект с тегом "Enemy" входит в радиус, меняем цвет на красный
                SetLineColor(Color.red);
                return; // Выходим из цикла, так как цвет уже установлен
            }
        }

        // Если цикл дошел до этого момента, значит, ни один враг не находится в радиусе, меняем цвет на желтый
        SetLineColor(Color.yellow);

        void SetLineColor(Color color)
        {
        // Установка цвета линии
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        }

    }

    void UpdateRadius(float radius)
    {
        float deltaTheta = (2f * Mathf.PI) / segments;
        float theta = 0f;

        Vector3[] points = new Vector3[segments + 1];

        for (int i = 0; i < segments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);

            points[i] = new Vector3(x, heightOffset, z);

            theta += deltaTheta;
        }

        lineRenderer.positionCount = segments + 1;
        lineRenderer.SetPositions(points);
    }
}



