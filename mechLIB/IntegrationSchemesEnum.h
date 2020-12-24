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
		enum class IntegrationSchemesEnum
	{
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Euler")]
#endif
	Euler,
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("Verlet")]
#endif
	Verlet,
#if (_MANAGED == 1) || (_M_CEE == 1)
		[Description("GearPC")]
#endif
	GearPC,
		maxIntegrationSchemesEnum
	};
};