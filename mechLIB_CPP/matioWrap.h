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
    void readFloatArr(const char* varName, std::vector<float>& pArr)
    {
        matvar_t* matVar = Mat_VarRead(mat, (char*)varName);
        if (matVar)
        {
            const float* xData = static_cast<const float*>(matVar->data);
            pArr.insert(pArr.end(), &xData[0], &xData[matVar->nbytes / matVar->data_size]);
        }
        Mat_VarFree(matVar);
        matVar = NULL;
    }
    static void writeFloatArr(const char* filename, const char* varName, std::vector<float>& pArr)
    {
        mat_t* matfp = NULL; //matfp contains pointer to MAT file or NULL on failure
        matfp = Mat_CreateVer(filename, NULL, MAT_FT_MAT5); //or MAT_FT_MAT4 / MAT_FT_MAT73
        size_t dim1d[1] = { pArr.size() };
        matvar_t* matVar = Mat_VarCreate(varName, MAT_C_SINGLE, MAT_T_SINGLE, 1, dim1d, &pArr.front(), 0); //rank 1
        Mat_VarWrite(matfp, matVar, MAT_COMPRESSION_NONE);
        Mat_VarFree(matVar);
        matVar = NULL;
        Mat_Close(matfp);
    }
};

