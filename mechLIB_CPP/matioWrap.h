#pragma once
#include <matio.h>
#include <string>
#include <vector>

class matioWrap
{
    mat_t* mat;
public:
    matioWrap(std::string& MATFile)
    {
        mat = Mat_Open(MATFile.c_str(), MAT_ACC_RDONLY);
        if (!mat)
        {
            char errBuff[2048];
            snprintf(errBuff, sizeof(errBuff), "Failed to open file: \"%s\"", MATFile.c_str());
            throw std::exception(errBuff);
        }
    }
    ~matioWrap()
    {
        if (mat)
        {
            Mat_Close(mat);
        }
        mat = NULL;
    }
    void readFloatArrFromMAT(const char* varName, std::vector<float> &pArr)
    {
        matvar_t* matVar = Mat_VarRead(mat, (char*)varName);
        if (matVar)
        {
            const float* xData = static_cast<const float*>(matVar->data);
            pArr.insert(pArr.end(), &xData[0], &xData[matVar->nbytes / matVar->data_size]);
        }
        matVar = NULL;
    }

};

