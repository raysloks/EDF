public class HealthScript
{
    public int current, max;

    public void Damage(int damage) {
        current -= damage;
        if (current > max)
            current = max;
    }
}
