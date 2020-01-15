#pragma once
#include "xyz_t.h"
typedef enum class N_t
{
	//coordinates
	p,
	//displacement
	u,
	//velocity
	v,
	//acceleration
	a
};
class deriv_t
{
public:
	//coordinates
	xyz_t p;
	//displacement
	xyz_t u;
	//velocity
	xyz_t v;
	//acceleration
	xyz_t a;
	deriv_t() :
		p({ 0,0,0 }),
		u({ 0,0,0 }),
		v({ 0,0,0 }),
		a({ 0,0,0 }) {}
	xyz_t GetByN(N_t N)
	{
		switch (N)
		{
		case N_t::p:
			return p;
		case N_t::u:
			return u;
		case N_t::v:
			return v;
		case N_t::a:
			return a;
		default:
			throw "Not expected derivative to get by";
		}
	}
	void SetByN(N_t N, xyz_t val)
	{
		switch (N)
		{
		case N_t::p:
			p = val;
			break;
		case N_t::u:
			u = val;
			break;
		case N_t::v:
			v = val;
			break;
		case N_t::a:
			a = val;
			break;
		default:
			throw "Not expected derivative to set by";
		}
	}
};