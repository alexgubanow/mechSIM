#pragma once
class PhModels
{
public:
	typedef enum class enm
	{
		hook,
		hookGeomNon,
		mooneyRiv
	};
	const char* getDescription(enm phMod) {
		switch (phMod)
		{
		case PhModels::enm::hook:
			return "Linear Hook model";
		case PhModels::enm::hookGeomNon:
			return "Hook model, nonlin geaometry";
		case PhModels::enm::mooneyRiv:
			return "Mooney-Rivlin model";
		default:
			return "";
		}
	}
};
