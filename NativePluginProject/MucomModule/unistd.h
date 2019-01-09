#pragma once

#include <direct.h>

int chdir(const char *dir)
{
	return _chdir(dir);
}