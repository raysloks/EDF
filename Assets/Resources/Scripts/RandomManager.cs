using System;

public class RandomManager
{
	public static Random random = new Random();
    public static Random ai = new Random();

	public static int d6()
	{
		return random.Next(1, 6);
	}
}