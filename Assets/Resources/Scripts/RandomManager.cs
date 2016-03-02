using System;

public class RandomManager
{
	public static Random random = new Random(DateTime.Now);

	public static int d6()
	{
		return random.Next(1, 6);
	}
}