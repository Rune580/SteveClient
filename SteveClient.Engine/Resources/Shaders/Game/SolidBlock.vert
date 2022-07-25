#version 460 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec3 aTangent;
layout(location = 3) in vec2 aTexCoord;
layout(location = 4) in float aAtlas;
layout(location = 5) in float aBlockPos;

layout(location = 0) flat out int atlas;
layout(location = 1) out vec2 texCoords;
layout(location = 2) out vec3 fNormal;
layout(location = 3) out vec3 FragPos;
layout(location = 4) out vec3 tangentLightDir;
layout(location = 5) out vec3 tangentViewPos;
layout(location = 6) out vec3 tangentFragPos;
layout(location = 7) out vec3 lightMapPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform vec3 viewPos;
uniform vec3 lightPos;

uniform int lightMapWidth;
uniform int lightMapHeight;

vec3 CalculateLightMapPos(int blockPos) {
    float width = lightMapWidth * 16;
    float height = lightMapHeight * 16;
    float depth = width * height;

    float y = int(floor(blockPos / depth)) / height;
    float z = (int(floor((blockPos - (y * depth)) / width)) / width);
    float x = (int(floor(blockPos - ((y * depth) + (z * width)))) / width);

    return vec3(x, y, z);
}

void main() {
    atlas = int(aAtlas);
    lightMapPos = CalculateLightMapPos(int(aBlockPos));
    texCoords = aTexCoord;
    FragPos = vec3(vec4(aPosition, 1.0) * model);

    mat3 normalMatrix = transpose(inverse(mat3(model)));
    vec3 T = normalize(normalMatrix * aTangent);
    vec3 N = normalize(normalMatrix * aNormal);
    T = normalize(T - dot(T, N) * N);
    vec3 B = cross(N, T);
    
    mat3 TBN = transpose(mat3(T, B, N));

    tangentLightDir = TBN * normalize(lightPos - FragPos);
    tangentViewPos = TBN * viewPos;
    tangentFragPos  = TBN * FragPos;

    fNormal = normalMatrix * aNormal;
    
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}
