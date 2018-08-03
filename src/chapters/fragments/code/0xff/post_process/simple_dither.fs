/*
	Simple Dithering
*/
precision highp float;

uniform sampler2D u_t0;
uniform sampler2D u_ditherTex;
uniform vec2 u_res;
uniform float u_time;

float ditherLookup(vec2 p){
	p.y = u_res.y - p.y;

	const float sz = 3.;
	//0,0,0,3,3,3,6,6,6
	vec2 cellIdx = floor(p/vec2(sz))*sz;

	vec2 median = cellIdx + 1.;//center
	vec2 texCoord = median/u_res;
	float medianCol = texture2D(u_t0, texCoord).x;

	// [0,0] [1,0] [2,0]
	// [0,1] [1,1] [2,1]
	// [0,2] [1,2] [2,2]
	vec2 localCoord = mod(p, vec2(3.));

	// use localCoord to sample dither matrix, 
	// stored as a texture

	localCoord /= 2.; // remap
	float ditherThreshold = texture2D(u_ditherTex, localCoord).x;

	// return texture2D(u_ditherTex, p).x;
	
	return step(medianCol, ditherThreshold);
	
	// if(localCoord.x < 1.){
	// 	if(localCoord.y < 1.){
	// 		return step(medianCol, 0.1);
	// 	}
	// 	if(localCoord.y < 2.){
	// 		return step(medianCol, 0.2);
	// 	}
	// 	if(localCoord.y < 3.){
	// 		return step(medianCol, 0.3);
	// 	}
	// }

	// if(localCoord.x < 2.){
	// 	if(localCoord.y < 1.){
	// 		return step(medianCol, 0.4);
	// 	}
	// 	if(localCoord.y < 2.){
	// 		return step(medianCol, 0.5);
	// 	}
	// 	if(localCoord.y < 3.){
	// 		return step(medianCol, 0.6);
	// 	}
	// }

	// if(localCoord.x < 3.){
	// 	if(localCoord.y < 1.){
	// 		return step(medianCol, 0.7);
	// 	}
	// 	if(localCoord.y < 2.){
	// 		return step(medianCol, 0.8);
	// 	}
	// 	if(localCoord.y < 3.){
	// 		return step(medianCol, 0.9);
	// 	}
	// }
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