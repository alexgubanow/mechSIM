#pragma once
namespace mechLIB {

#if (_MANAGED == 1) || (_M_CEE == 1)
	public
#endif
		enum class Derivatives
	{
		//coordinates
		p,

		//displacement
		u,

		//velocity
		v,

		//acceleration
		a,		
		f, maxDerivatives
	};

}