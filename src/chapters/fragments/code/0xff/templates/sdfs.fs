// SDFs

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float sdSphere(vec3 p, float r){
	return length(p)-r;
}


float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max( d , 0. ));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float sdBox(vec2 p, vec2 sz){
}

float sdCone(vec3 p, vec2 sz){
	
}