vec2 skew(vec2 p, float x){
  p.x+= p.y*x;
  return p;
}