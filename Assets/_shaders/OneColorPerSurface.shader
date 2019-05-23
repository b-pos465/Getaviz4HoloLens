Shader "Custom/OneColorPerSurface"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
	}

		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f {
				half3 worldNormal : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.worldNormal = UnityObjectToWorldNormal(normal);
				return o;
			}

			fixed4 _Color;
			fixed4 _OrthogonalColor;

			fixed4 frag(v2f i) : SV_Target
			{
				float3 cameraForward = mul((float3x3) unity_CameraToWorld, float3(0, 0, -1));

				float dotProduct = dot(i.worldNormal, cameraForward);

				fixed4 c = 0;
				c.rgb = _Color * ((dotProduct * 0.6f) + 0.4f);
				return c;
		}
		ENDCG
	}
	}
}
