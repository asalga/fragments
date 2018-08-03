/*
	Simple Dithering
*/
precision highp float;

uniform sampler2D u_t0;
uniform vec2 u_res;


float ditherLookup(vec2 p){
	p.y = u_res.y - p.y;
	const float sz = 3.;

	//0,0,0,3,3,3,6,6,6
	vec2 cellIdx = floor(p/vec2(sz))*sz;
	
	// ivec2 localCell = ivec2(0);

	vec2 median = cellIdx + 1.;
	vec2 texCoord = median/u_res;
	float medianCol = texture2D(u_t0, texCoord).x;

	vec2 localCoord = mod(p, vec2(3.));
	// localCoord.x = 0.;
	// localCoord.y = 0.;
	
	// if(localCoord.x <= 1. && localCoord.y <= 1.){

	// if(all(lessThan(localCoord, vec2(1., 1.)))){
	
	if(localCoord.x < 1.){
		if(localCoord.y < 1.){
			return step(medianCol, 0.1);
		}
		if(localCoord.y < 2.){
			return step(medianCol, 0.2);
		}
		if(localCoord.y < 3.){
			return step(medianCol, 0.3);
		}
	}

	if(localCoord.x < 2.){
		if(localCoord.y < 1.){
			return step(medianCol, 0.4);
		}
		if(localCoord.y < 2.){
			return step(medianCol, 0.5);
		}
		if(localCoord.y < 3.){
			return step(medianCol, 0.6);
		}
	}

	if(localCoord.x < 3.){
		if(localCoord.y < 1.){
			return step(medianCol, 0.7);
		}
		if(localCoord.y < 2.){
			return step(medianCol, 0.8);
		}
		if(localCoord.y < 3.){
			return step(medianCol, 0.9);
		}
	}
	// if(all(lessThan(localCoord, vec2(2., 1.)))){
	// 	return step(medianCol, 0.9);
	// }
	// if(all(lessThan(localCoord, vec2(3., 1.)))){
	// 	return step(medianCol, 0.4);
	// }
	// if(all(lessThan(localCoord, vec2(1., 2.)))){
	// 	return step(medianCol, 0.2);
	// }
	// if(all(lessThan(localCoord, vec2(2., 2.)))){
	// 	return step(medianCol, 0.1);
	// }
	// if(all(lessThan(localCoord, vec2(3., 2.)))){
	// 	return step(medianCol, 0.3);
	// }
	// if(all(lessThan(localCoord, vec2(1., 3.)))){
	// 	return step(medianCol, 0.5);
	// }
	// if(all(lessThan(localCoord, vec2(2., 3.)))){
	// 	return step(medianCol, 0.2);
	// }
	// if(all(lessThan(localCoord, vec2(3., 3.)))){
	// 	return step(medianCol, .7);
	// }


	// if(all(equal(localCoord, vec2(0., 2.)))){
	// 	return step(medianCol, 0.3);
	// }

	// if(all(equal(localCoord, vec2(1., 0.)))){
	// 	return step(0.4, medianCol);
	// }

	// return 0.;

return 1.;
	// const float localx = mod(p.x, 3.0);
	// ivec2 localCoord = ivec2(0);
	// localCoord.x = mod(int(p.x), 3);
	// int i = mod(1, 2);
	// mod( ivec2(p), ivec2(0));
	//mod(ivec2(p), ivec2(3));

	// ditherMat[localCell.x][localCell.y];

	
}


void main(){	
	vec2 p = gl_FragCoord.xy/u_res;

	float i = ditherLookup(gl_FragCoord.xy);

	gl_FragColor = vec4(vec3(i),1);
}