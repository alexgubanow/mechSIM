#pragma once
namespace mechLIB_CPPWrapper {

#if (_MANAGED == 1) || (_M_CEE == 1)
    public
#endif
        enum class NodeFreedom
    {
        x, y, z, xy, xz, yz, xyz, locked
    };

}

