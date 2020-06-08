#pragma once
namespace mechLIB_CPP {

#if (_MANAGED == 1) || (_M_CEE == 1)
	public
#endif
		enum class NodeLoad
	{
		//coordinates
		p,

		//displacement
		u,

		//velocity
		v,

		//acceleration
		a,
		
		f,
		none
	};

}

