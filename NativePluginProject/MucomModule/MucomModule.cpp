// MucomModule.cpp : DLL アプリケーション用にエクスポートされる関数を定義します。
//

#include "stdafx.h"

#include <exception>
#include <stdint.h>

#include <IUnityInterface.h>

#include "mod/mucom_module.h"

template<typename RetT, typename ... Args>
struct Proxy
{
	template<typename RetT(MucomModule::*func)(Args...)>
	static RetT UNITY_INTERFACE_API Func(MucomModule *self, Args... args)
	{
		return (self->*func)(args...);
	}
};

#ifdef __cplusplus
extern "C" {
#endif


void DestroyMucom(MucomModule *self)
{
	delete self;
}

int32_t UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API CreateMucomModulePluginInstance(
	void **buffer, int32_t bufferSize) noexcept try
{
	typedef void * void_ptr;

	static const void_ptr funcs[] =
	{
		DestroyMucom,
		Proxy<bool, const char *, const char *>::Func<&MucomModule::Open>,
		Proxy<void, int>::Func<&MucomModule::SetRate>,
		Proxy<void, const char *>::Func<&MucomModule::SetPCM>,
		Proxy<void, const char *>::Func<&MucomModule::SetVoice>,
		Proxy<void, short *, int>::Func<&MucomModule::Mix>,
		Proxy<bool>::Func<&MucomModule::Play>,
		Proxy<void>::Func<&MucomModule::Close>,
		Proxy<const char *>::Func<&MucomModule::GetResult>,
	};

	static constexpr int32_t requiredSize = sizeof(funcs) / sizeof(void *) + 1;

	if (buffer == nullptr || bufferSize < 1)
	{
		return requiredSize;
	}

	if (bufferSize < requiredSize)
	{
		return 0;
	}

	auto plugin = new MucomModule();
	buffer[0] = plugin;
	int32_t index = 1;
	for (auto p : funcs)
	{
		buffer[index] = p;
		index++;
	}


	return requiredSize;
}
catch (const std::exception&)
{
	return 0;
}

#ifdef __cplusplus
}
#endif



