// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.08471543,fgcg:0.0648789,fgcb:0.1470588,fgca:1,fgde:0.1,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:6420,x:35887,y:33474,varname:node_6420,prsc:2|emission-9878-OUT,alpha-7055-OUT,clip-3339-OUT;n:type:ShaderForge.SFN_Multiply,id:7212,x:33241,y:33176,varname:node_7212,prsc:2|A-1397-A,B-4659-OUT,C-7983-OUT,D-1914-R;n:type:ShaderForge.SFN_Slider,id:7983,x:32579,y:33092,ptovrint:False,ptlb:Noise multiplier,ptin:_Noisemultiplier,varname:node_7983,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:20,max:20;n:type:ShaderForge.SFN_Time,id:8794,x:30558,y:33191,varname:node_8794,prsc:2;n:type:ShaderForge.SFN_Multiply,id:401,x:30967,y:33070,varname:node_401,prsc:2|A-8794-T,B-770-OUT,C-7864-OUT;n:type:ShaderForge.SFN_Tex2d,id:1914,x:32883,y:33355,varname:node_1914,prsc:2,ntxv:0,isnm:False|TEX-1230-TEX;n:type:ShaderForge.SFN_Slider,id:770,x:30502,y:33035,ptovrint:False,ptlb:Fire Speed,ptin:_FireSpeed,varname:node_770,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:2;n:type:ShaderForge.SFN_VertexColor,id:1397,x:32579,y:32852,varname:node_1397,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:9716,x:35149,y:33458,ptovrint:False,ptlb:Brightness Multiplier,ptin:_BrightnessMultiplier,varname:node_9716,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2dAsset,id:903,x:30423,y:33387,ptovrint:False,ptlb:noise,ptin:_noise,varname:node_903,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4322,x:31500,y:33269,varname:node_4322,prsc:2,ntxv:0,isnm:False|UVIN-3796-UVOUT,TEX-903-TEX;n:type:ShaderForge.SFN_Tex2d,id:1590,x:31500,y:33391,varname:node_1590,prsc:2,ntxv:0,isnm:False|UVIN-4078-UVOUT,TEX-903-TEX;n:type:ShaderForge.SFN_Panner,id:3796,x:31311,y:33119,varname:node_3796,prsc:2,spu:0,spv:-0.5|UVIN-5847-OUT,DIST-401-OUT;n:type:ShaderForge.SFN_Panner,id:4078,x:31306,y:33516,varname:node_4078,prsc:2,spu:0,spv:-1|UVIN-5847-OUT,DIST-401-OUT;n:type:ShaderForge.SFN_Clamp01,id:3339,x:35530,y:33771,varname:node_3339,prsc:2|IN-5457-OUT;n:type:ShaderForge.SFN_TexCoord,id:3333,x:32499,y:33733,varname:node_3333,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:368,x:33531,y:33231,varname:node_368,prsc:2|A-7212-OUT,B-2342-OUT;n:type:ShaderForge.SFN_Add,id:5933,x:33790,y:33240,varname:node_5933,prsc:2|A-6892-OUT,B-368-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:1230,x:32552,y:33378,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_1230,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2342,x:33253,y:33302,varname:node_2342,prsc:2|A-1914-G,B-5740-OUT;n:type:ShaderForge.SFN_OneMinus,id:5740,x:32831,y:33862,varname:node_5740,prsc:2|IN-3333-V;n:type:ShaderForge.SFN_Step,id:5457,x:34347,y:33511,varname:node_5457,prsc:2|A-8847-OUT,B-8480-OUT;n:type:ShaderForge.SFN_Add,id:1985,x:33943,y:33859,varname:node_1985,prsc:2|A-1794-OUT,B-3333-V;n:type:ShaderForge.SFN_Clamp01,id:8847,x:34159,y:33859,varname:node_8847,prsc:2|IN-1985-OUT;n:type:ShaderForge.SFN_Multiply,id:6892,x:33531,y:33484,varname:node_6892,prsc:2|A-1914-B,B-9224-OUT;n:type:ShaderForge.SFN_Slider,id:9224,x:33199,y:33558,ptovrint:False,ptlb:Base Size,ptin:_BaseSize,varname:node_9224,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:3;n:type:ShaderForge.SFN_Multiply,id:8480,x:33987,y:33550,varname:node_8480,prsc:2|A-5933-OUT,B-5740-OUT;n:type:ShaderForge.SFN_Step,id:2459,x:34647,y:33744,varname:node_2459,prsc:2|A-3476-OUT,B-1593-OUT;n:type:ShaderForge.SFN_Slider,id:3476,x:34235,y:33713,ptovrint:False,ptlb:Inner Step,ptin:_InnerStep,varname:node_3476,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:3;n:type:ShaderForge.SFN_Multiply,id:7234,x:34942,y:33535,varname:node_7234,prsc:2|A-2459-OUT,B-1698-RGB;n:type:ShaderForge.SFN_Multiply,id:1593,x:34443,y:34069,varname:node_1593,prsc:2|A-8480-OUT,B-434-OUT;n:type:ShaderForge.SFN_OneMinus,id:7055,x:34995,y:34113,varname:node_7055,prsc:2|IN-2459-OUT;n:type:ShaderForge.SFN_Multiply,id:4723,x:35305,y:34065,varname:node_4723,prsc:2|A-7055-OUT,B-3290-RGB;n:type:ShaderForge.SFN_Add,id:5397,x:35206,y:33594,varname:node_5397,prsc:2|A-7234-OUT,B-4723-OUT;n:type:ShaderForge.SFN_Multiply,id:9878,x:35459,y:33609,varname:node_9878,prsc:2|A-5397-OUT,B-9716-OUT;n:type:ShaderForge.SFN_Color,id:3290,x:35088,y:34315,ptovrint:False,ptlb:Outer Colour Base,ptin:_OuterColourBase,varname:node_7835,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.7655172,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:1698,x:34694,y:33320,ptovrint:False,ptlb:Inner Colour Base,ptin:_InnerColourBase,varname:_OuterColourBase_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.7655172,c3:0,c4:1;n:type:ShaderForge.SFN_Add,id:3965,x:31795,y:33502,varname:node_3965,prsc:2|A-4322-R,B-1590-R;n:type:ShaderForge.SFN_TexCoord,id:1817,x:30627,y:33498,varname:node_1817,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:5847,x:30996,y:33632,varname:node_5847,prsc:2|A-1817-UVOUT,B-7864-OUT;n:type:ShaderForge.SFN_Slider,id:7864,x:30440,y:33744,ptovrint:False,ptlb:Noise Scale,ptin:_NoiseScale,varname:node_7864,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Power,id:4659,x:32032,y:33601,varname:node_4659,prsc:2|VAL-3965-OUT,EXP-8556-OUT;n:type:ShaderForge.SFN_Slider,id:8556,x:31778,y:33763,ptovrint:False,ptlb:Distorition Power,ptin:_DistoritionPower,varname:node_8556,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:10;n:type:ShaderForge.SFN_Multiply,id:9504,x:33605,y:34080,varname:node_9504,prsc:2|A-1914-G,B-5740-OUT;n:type:ShaderForge.SFN_Power,id:434,x:33888,y:34117,varname:node_434,prsc:2|VAL-9504-OUT,EXP-4175-OUT;n:type:ShaderForge.SFN_Slider,id:4175,x:33640,y:34321,ptovrint:False,ptlb:Inner Vertical Falloff,ptin:_InnerVerticalFalloff,varname:node_4175,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:5;n:type:ShaderForge.SFN_Vector1,id:1794,x:33755,y:33965,varname:node_1794,prsc:2,v1:0.5;proporder:770-9716-903-7983-7864-1230-9224-3476-3290-1698-8556-4175;pass:END;sub:END;*/

Shader "Custom/FX/FireStepNoiseBase" {
    Properties {
        _FireSpeed ("Fire Speed", Range(0, 2)) = 0
        _BrightnessMultiplier ("Brightness Multiplier", Float ) = 1
        _noise ("noise", 2D) = "white" {}
        _Noisemultiplier ("Noise multiplier", Range(0, 20)) = 20
        _NoiseScale ("Noise Scale", Range(0, 1)) = 1
        _Mask ("Mask", 2D) = "white" {}
        _BaseSize ("Base Size", Range(0, 3)) = 1
        _InnerStep ("Inner Step", Range(0, 3)) = 0
        _OuterColourBase ("Outer Colour Base", Color) = (1,0.7655172,0,1)
        _InnerColourBase ("Inner Colour Base", Color) = (1,0.7655172,0,1)
        _DistoritionPower ("Distorition Power", Range(1, 10)) = 1
        _InnerVerticalFalloff ("Inner Vertical Falloff", Range(1, 5)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Noisemultiplier;
            uniform float _FireSpeed;
            uniform float _BrightnessMultiplier;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _BaseSize;
            uniform float _InnerStep;
            uniform float4 _OuterColourBase;
            uniform float4 _InnerColourBase;
            uniform float _NoiseScale;
            uniform float _DistoritionPower;
            uniform float _InnerVerticalFalloff;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 node_1914 = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 node_8794 = _Time + _TimeEditor;
                float node_401 = (node_8794.g*_FireSpeed*_NoiseScale);
                float2 node_5847 = (i.uv0*_NoiseScale);
                float2 node_3796 = (node_5847+node_401*float2(0,-0.5));
                float4 node_4322 = tex2D(_noise,TRANSFORM_TEX(node_3796, _noise));
                float2 node_4078 = (node_5847+node_401*float2(0,-1));
                float4 node_1590 = tex2D(_noise,TRANSFORM_TEX(node_4078, _noise));
                float node_4659 = pow((node_4322.r+node_1590.r),_DistoritionPower);
                float node_5740 = (1.0 - i.uv0.g);
                float node_8480 = (((node_1914.b*_BaseSize)+((i.vertexColor.a*node_4659*_Noisemultiplier*node_1914.r)*(node_1914.g*node_5740)))*node_5740);
                clip(saturate(step(saturate((0.5+i.uv0.g)),node_8480)) - 0.5);
////// Lighting:
////// Emissive:
                float node_1593 = (node_8480*pow((node_1914.g*node_5740),_InnerVerticalFalloff));
                float node_2459 = step(_InnerStep,node_1593);
                float node_7055 = (1.0 - node_2459);
                float3 emissive = (((node_2459*_InnerColourBase.rgb)+(node_7055*_OuterColourBase.rgb))*_BrightnessMultiplier);
                float3 finalColor = emissive;
                return fixed4(finalColor,node_7055);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Noisemultiplier;
            uniform float _FireSpeed;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _BaseSize;
            uniform float _NoiseScale;
            uniform float _DistoritionPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 node_1914 = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 node_8794 = _Time + _TimeEditor;
                float node_401 = (node_8794.g*_FireSpeed*_NoiseScale);
                float2 node_5847 = (i.uv0*_NoiseScale);
                float2 node_3796 = (node_5847+node_401*float2(0,-0.5));
                float4 node_4322 = tex2D(_noise,TRANSFORM_TEX(node_3796, _noise));
                float2 node_4078 = (node_5847+node_401*float2(0,-1));
                float4 node_1590 = tex2D(_noise,TRANSFORM_TEX(node_4078, _noise));
                float node_4659 = pow((node_4322.r+node_1590.r),_DistoritionPower);
                float node_5740 = (1.0 - i.uv0.g);
                float node_8480 = (((node_1914.b*_BaseSize)+((i.vertexColor.a*node_4659*_Noisemultiplier*node_1914.r)*(node_1914.g*node_5740)))*node_5740);
                clip(saturate(step(saturate((0.5+i.uv0.g)),node_8480)) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
