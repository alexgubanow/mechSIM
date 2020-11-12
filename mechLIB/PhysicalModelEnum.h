#pragma once
#if (_MANAGED == 1) || (_M_CEE == 1)
#include "EnumDescriptionTypeConverter.h"
using System::ComponentModel::DescriptionAttribute;
using System::ComponentModel::TypeConverterAttribute;
#endif


namespace mechLIB {
#if (_MANAGED == 1) || (_M_CEE == 1)
	[TypeConverter(EnumDescriptionTypeConverter::typeid)]
	public
#endif
		enum class PhysicalModelEnum
	{
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Linear Hook")]
#endif
	hook,
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Nonlinear Hook")]
#endif
	hookGeomNon,
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Mooney-Rivlin")]
#endif
	mooneyRiv,
		maxModel
	};
};