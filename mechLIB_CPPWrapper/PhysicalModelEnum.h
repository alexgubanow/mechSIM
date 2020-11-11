#pragma once
#if (_MANAGED == 1) || (_M_CEE == 1)
using System::ComponentModel::DescriptionAttribute;
#endif

namespace mechLIB_CPP {

#if (_MANAGED == 1) || (_M_CEE == 1)
	public
#endif
		enum class PhysicalModelEnum
	{
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("linear Hook model")]
#endif
	hook,
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Hook model, nonlin geaometry")]
#endif
	hookGeomNon,
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Mooney-Rivlin model")]
#endif
	mooneyRiv,
		maxModel
	};

}
