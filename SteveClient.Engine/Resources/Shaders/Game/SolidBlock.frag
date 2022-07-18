#version 460 core

out vec4 FragColor;

layout(location = 0) flat in int atlas;
layout(location = 1) in vec2 texCoords;
layout(location = 2) in vec3 Normal;
layout(location = 3) in vec3 FragPos;
layout(location = 4) in vec3 tangentLightDir;
layout(location = 5) in vec3 tangentViewPos;
layout(location = 6) in vec3 tangentFragPos;

uniform vec4 tint;
uniform sampler2DArray textureSampler;
uniform vec3 viewPos;
uniform vec3 lightPos;

uniform float ambientStrength;
uniform float specularStrength;

vec3 getLightDir() {
    return normalize(lightPos - FragPos);
}

vec3 getLightColor() {
    return vec3(1, 1, 1);
}

vec3 getAmbient() {
    vec3 lightCol = getLightColor();
    
    return lightCol * ambientStrength;
}

vec3 getDiffuse() {
    vec3 lightDir = getLightDir();
    vec3 normal = normalize(Normal);
    
    float diff = max(dot(lightDir, normal), 0.0);

    return diff * getLightColor();
}

vec3 getSpec() {
    vec3 lightDir = getLightDir();
    vec3 normal = normalize(Normal);
    
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, normal);
    
    float spec = 0.0;
    
    vec3 halfDir = normalize(lightDir + viewDir);
    spec = pow(max(dot(normal, halfDir), 0.0), 32.0);
    
    return specularStrength * spec * getLightColor(); 
}

void main() {
    vec2 uv = texCoords;
    
    if(uv.x > 1.0 || uv.y > 1.0 || uv.x < 0.0 || uv.y < 0.0)
        discard;
    
    vec4 color = texture(textureSampler, vec3(uv, atlas)) * vec4(tint.rgb, 1);
    if (color.a == 0)
        discard;

    vec3 ambient = getAmbient();
    vec3 diffuse = getDiffuse();
    vec3 specular = getSpec();
    
    color.rgb = (ambient + diffuse + specular) * color.rgb;
    
    FragColor = color;
}
