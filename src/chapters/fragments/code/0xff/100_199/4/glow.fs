void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
  vec2 p = ( fragCoord.xy * 2.0 - iResolution.xy) /
        min(iResolution.x,iResolution.y);

  float f = 0.0;

  for ( float i = 0.0; i < 410.0; i++){

    float s =  sin(iTime * 4. + i * 0.031415926)/10.;

    float c = cos(iTime * 2. + i * 0.031415926)/1.;

        float glow = (pow(i,2.1) / 4000.0 + 10.*sin(iTime/2.));

    f += 0.00001/abs(length(p+vec2(c,s))-i/1000.)* glow;


  }

  fragColor = vec4(vec3(f),1.0);
}