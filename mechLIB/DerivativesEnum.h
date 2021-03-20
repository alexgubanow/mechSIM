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
		enum class DerivativesEnum
	{
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Coordinates")]
#endif
	p,
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Velocity")]
#endif
	v,
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Acceleration")]
#endif
	a,
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Force")]
#endif
	f,
		DerivativesEnumMAX
	};
};