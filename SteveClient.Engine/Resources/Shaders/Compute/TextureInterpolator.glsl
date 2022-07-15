#version 460 core

uniform sampler2DArray textures;
uniform float weight;

layout(local_size_x = 8, local_size_y = 4, local_size_z = 1) in;
layout(rgba8, binding = 0) uniform image2D interpolatedTexture;

void main() {
    ivec2 pos = ivec2(gl_GlobalInvocationID.xy);
    vec2 uv = vec2(pos.x / 16f, pos.y / 16f);
    
    vec4 tex1 = texture(textures, vec3(uv, 0));
    vec4 tex2 = texture(textures, vec3(uv, 1));
    
    vec4 color = mix(tex1, tex2, weight);
    
    imageStore(interpolatedTexture, pos, color);
}
