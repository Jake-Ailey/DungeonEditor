#include <iostream>

// pass-by reference

void changeHealth(int* health, int change)
{
	health -= change;
}

int main()
{
	int health = 100;

	std::cout << health << std::endl;

	changeHealth(&health, 50);

	std::cout << health << std::endl;
}