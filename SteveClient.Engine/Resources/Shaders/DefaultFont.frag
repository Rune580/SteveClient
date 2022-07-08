#version 330 core

out vec4 FragColor;

in vec2 texCoord;

uniform vec4 color;
uniform sampler2D textureSampler;

void main() {
    float dist = 0.5 - texture(textureSampler, texCoord).r;
    
    ivec2 size = textureSize(textureSampler, 0);
    float dx = dFdx(texCoord.x) * size.x;
    float dy = dFdy(texCoord.y) * size.y;
    float toPixels = 24.0 * inversesqrt(dx * dx + dy * dy);
    
    float alpha = 1 - clamp(dist * toPixels + 0.5, 0.0, 1.0);
    if (alpha == 0)
        discard;
    
    vec4 text = vec4(1, 1, 1, alpha);
    FragColor = color * text;
}
