#pragma once
#include <matio.h>
#include <string>
#include <vector>

class matReader
{
    mat_t* matFile;
public:
    matReader(const char* MATFile)
    {
        matFile = Mat_Open(MATFile, MAT_ACC_RDONLY);
        if (!matFile)
        {
            char errBuff[2048];
            snprintf(errBuff, sizeof(errBuff), "Failed to open file: \"%s\"", MATFile);
            throw (const char*)errBuff;
        }
    }
    ~matReader()
    {
        if (matFile)
        {
            Mat_Close(matFile);
        }
        matFile = NULL;
    }
    void readFloatArr(const char* varName, std::vector<float>& pArr)
    {
        matvar_t* matVar = Mat_VarRead(matFile, (char*)varName);
        if (matVar)
        {
            const float* xData = static_cast<const float*>(matVar->data);
            pArr.insert(pArr.end(), &xData[0], &xData[matVar->nbytes / matVar->data_size]);
        }
        Mat_VarFree(matVar);
        matVar = NULL;
    }
};


class matWriter
{
    mat_t* matFile;
public:
    matWriter(const char* MATFile)
    {
        matFile = Mat_CreateVer(MATFile, NULL, MAT_FT_DEFAULT);
        if (!matFile)
        {
            char errBuff[2048];
            snprintf(errBuff, sizeof(errBuff), "Failed to create file: \"%s\"", MATFile);
            throw (const char*)errBuff;
        }
    }
    ~matWriter()
    {
        if (matFile)
        {
            Mat_Close(matFile);
        }
        matFile = NULL;
    }
    void writeFloatArr(const char* varName, std::vector<float>& value)
    {
        size_t dim1d[1] = { value.size() };
        matvar_t* matVar = Mat_VarCreate(varName, MAT_C_SINGLE, MAT_T_SINGLE, 1, dim1d, &value.front(), 0);
        Mat_VarWrite(matFile, matVar, MAT_COMPRESSION_NONE);
        Mat_VarFree(matVar);
        matVar = NULL;
    }
    void writeString(const char* varName,const std::string& value)
    {
        size_t dim[2] = { 1, value.size() };
        matvar_t* matVar = Mat_VarCreate(varName, MAT_C_CHAR, MAT_T_UTF8, 2, dim, (void *)&value.front(), 0);
        Mat_VarWrite(matFile, matVar, MAT_COMPRESSION_NONE);
        Mat_VarFree(matVar);
        matVar = NULL;
    }
    void writeFloat(const char* varName, float value)
    {
        size_t dim[2] = { 1, 1 };
        matvar_t* matVar = Mat_VarCreate(varName, MAT_C_SINGLE, MAT_T_SINGLE, 2, dim, &value, 0);
        Mat_VarWrite(matFile, matVar, MAT_COMPRESSION_NONE);
        Mat_VarFree(matVar);
        matVar = NULL;
    }
    void writeINT32(const char* varName, int value)
    {
        size_t dim[2] = { 1, 1 };
        matvar_t* matVar = Mat_VarCreate(varName, MAT_C_INT32, MAT_T_INT32, 2, dim, &value, 0);
        Mat_VarWrite(matFile, matVar, MAT_COMPRESSION_NONE);
        Mat_VarFree(matVar);
        matVar = NULL;
    }
    void writeUINT64(const char* varName, size_t value)
    {
        size_t dim[2] = { 1, 1 };
        matvar_t* matVar = Mat_VarCreate(varName, MAT_C_UINT64, MAT_T_UINT64, 2, dim, &value, 0);
        Mat_VarWrite(matFile, matVar, MAT_COMPRESSION_NONE);
        Mat_VarFree(matVar);
        matVar = NULL;
    }
};

