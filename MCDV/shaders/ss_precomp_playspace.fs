#version 330 core
//                                         OPENGL
// ____________________________________________________________________________________________
in vec2 TexCoords;
out vec4 FragColor;


//                                     SAMPLER UNIFORMS
// Image Inputs _______________________________________________________________________________
uniform sampler2D tex_in;	// Background texture

//                                       SHADER HELPERS
// ____________________________________________________________________________________________
//     ( Simple sample with offset )
vec2 pixel_size = 1.0 / vec2(textureSize(tex_in, 0));
vec4 getSample(vec2 offset)
{
	return vec4(texture(tex_in, TexCoords + (offset * pixel_size)));
}

//                                       SHADER PROGRAM
// ____________________________________________________________________________________________
//     ( Write all your shader code & functions here )

void main()
{
	vec4 sIn = getSample(vec2(0,0));
	if(sIn.r > 0.0)
	{
		FragColor = sIn;
	} 
	else
	{
		//Temp
		int sampleCount = 32;
		int outlineWidth = 2;

		// Glow sampler ==========================================================
		float sT = 0;
		for(int x = 0; x <= sampleCount; x++)
		{
			for(int y = 0; y <= sampleCount; y++)
			{
				sT += getSample(vec2(-16 + x,-16 + y)).r;
			}
		}

		//Adjust slightly
		sT *= 2;
		sT /= (sampleCount * sampleCount);

		// Outline sampler =======================================================
		float olT = 0;
		for(int x = 0; x <= outlineWidth * 2; x++)
		{
			for(int y = 0; y <= outlineWidth * 2; y++)
			{
				olT += getSample(vec2(-outlineWidth + x,-outlineWidth + y)).r;
			}
		}
		
		FragColor = vec4(sIn.r, sIn.g, sT, olT);
	}
}