//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED
void BlurFunction_float(UnityTexture2D tex, UnitySamplerState samplerState, float2 uv, float blurSize, out float output)
{
    //float mean = 0;
    //for (int i = 0; i < int(blurSize); i++) {
    //    for (int j = 0; j < int(blurSize); j++) {
    //        float2 sampleUV = float2(uv.x + i - int(blurSize / 2.0), uv.y + j - int(blurSize / 2.0));
    //        mean += SAMPLE_TEXTURE2D_LOD(tex, samplerState, sampleUV, 0).r;
    //    }
    //}
    //output = mean;

    float sum = 0.0;
    float total = 0.0;

    float2 offsets[9] = {
    float2(-1.0, -1.0), float2(0.0, -1.0), float2(1.0, -1.0),
    float2(-1.0, 0.0),  float2(0.0, 0.0),  float2(1.0, 0.0),
    float2(-1.0, 1.0),  float2(0.0, 1.0),  float2(1.0, 1.0)
    };

    for (int i = 0; i < 9; i++)
    {
        float2 sampleUV = uv + offsets[i] * blurSize;
        sum += SAMPLE_TEXTURE2D_LOD(tex, samplerState, sampleUV, 0).r;
        total += 1.0;
    }

    output = saturate(sum / total);
}
#endif //MYHLSLINCLUDE_INCLUDED