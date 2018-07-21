/*
    Andor Saga
*/
'use strict';

let globalVs = `
precision highp float;
attribute vec3 aPosition;

uniform mat4 uModelViewMatrix;
uniform mat4 uProjectionMatrix;

void main() {
  gl_Position = uProjectionMatrix * uModelViewMatrix * vec4(aPosition, 1.0 );
}`;


let gif, YesMakeGif = false;

Number.prototype.clamp = function(min, max) {
  return Math.min(Math.max(this, min), max);
};

function animate(time) {
  requestAnimationFrame(animate);
  // TWEEN.update(time);
}
requestAnimationFrame(animate);


/*
  fs - array of frag shaders
*/
function makeSketch(fs, params) {
  const DefaultSketchWidth = 320;
  const DefaultSketchHeight = 240;

  let w, h;
  let img0, img1, img2;

  let timeVal = { t: 0 };
  let sketchTime;
  let tracking = [];
  let easing = 0.05;
  let start = 0;
  let mouseIsDown = 0;
  let lastMouseDown = [120, 200];
  // let shader_0_Frag, shader_1_Frag;

  let gfx, shader_0, shader_1;
  let shader_0_Frag = fs[0];
  let shader_1_Frag = fs[1];


  var sketch = function(p) {

    p.preload = function() {
      let mainFs = `precision mediump float;
                    uniform sampler2D lastBuffer;
                    void main(){
                      vec2 p = gl_FragCoord.xy/vec2(300.);
                      vec4 col = texture2D(lastBuffer, vec2(0.5));
                      gl_FragColor = vec4(col.rg,1, 1);
                    }`;
    };

    p.setup = function() {
      w = params.width || DefaultSketchWidth;
      h = params.height || DefaultSketchHeight;
      sketchTime = 0;
      tracking = [0, 0];

      p.createCanvas(w, h, p.WEBGL);
      gfx = p.createGraphics(w, h, p.WEBGL);

      shader_0 = new p5.Shader(gfx._renderer, globalVs, shader_0_Frag);
      shader_1 = new p5.Shader(p._renderer, globalVs, shader_1_Frag);

      p.pixelDensity(1);
      $(p.canvas).appendTo($('#target'));
      p.loop();
    };


    /**
          Draw
    */
    p.draw = function() {
      let width = w;
      let height = h;
      let sz = 1.;

      gfx.push();
      gfx.translate(width / 2, height / 2);
      gfx.shader(shader_0);
      shader_0.setUniform('u_res', [width, height]);
      shader_0.setUniform('u_time', p.millis() / 1000.);
      gfx.rect(-width * sz, -height * sz, width * sz, height * sz, 2, 2);
      gfx.pop();

      p.push();
      p.translate(width / 2, height / 2);
      p.shader(shader_1);

      shader_1.setUniform('col', [1,1, 1]);
      shader_1.setUniform('res', [width, height]);
      shader_1.setUniform('time', p.millis() / 1000.);
      shader_1.setUniform('_', [ -1,  -1,   0, 
                                 -1,   1,  -1,
                                 -1,   0,   0,
                                   0,  1,   0, 
                                  -1,  1,   0, 
                                   1,  1,  1]);
      shader_1.setUniform('t1', gfx);
      p.rect(-width * sz, -height * sz, width * sz, height * sz, 2, 2);
      p.pop();

    }; //end draw
  };
  return sketch;
}

let demo = {
  '0': {
    src: '../fragments/code/0xff/100_199/0/108_seed_of_life.fs'
  },
  '1': {
    src: '../fragments/code/0xff/90/99/1.fs'
  }
};

function getFs0() {
  return fetch(demo[0].src)
    .then(res => res.text())
}

function getFs1() {
  return fetch(demo[1].src)
    .then(res => res.text())
}

(function load() {
  Promise.all([getFs0(), getFs1()])
    .then(fragShaders => {
      let relPath;
      let sketch = new p5(makeSketch(fragShaders, {width:500, height: 500}), relPath);
    });
})();