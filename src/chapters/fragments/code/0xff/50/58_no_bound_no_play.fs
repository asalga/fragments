// 58 - "No bounce, no play"

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	float speed = 50.;	
	float time = iTime * .1;
	float falloff = 1.3;
	
	const int NumMetaBalls = 28;
	vec3 metaBalls[NumMetaBalls];

	float d = 0.;
	vec2 screen;
	float c = 0.;
	
	for(int i = 0; i < NumMetaBalls; i++){
		vec2 pos = hashPosition(float(i))+
					vec2(time * speed * (1. + float(i)));
		
		screen = floor(mod(pos/res, vec2(2.)));
	
		// remap 0..1 to -1 to 1 to avoid branching in next line.
		vec2 dir = screen * 2. - 1.;
		vec2 finalPos = vec2(res * (vec2(1.) - screen) + dir * mod(pos, res));
		
		metaBalls[i] = vec3(finalPos, abs((float(i))));
		metaBalls[i].z = res.y / 200. * float(i) * 0.5;
		
		d = metaBalls[i].z / distance(metaBalls[i].xy , fragCoord.xy);
		c += pow(d, falloff);
	}
	
	// remove blurring
	float c2 = (c < 1.) ? 0. : c;
	
	fragColor = vec4(c2, c + 0.2, c, 1);
}