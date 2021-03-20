function force = GetElasticForce(k, restLenght, newLength)
    force = k * (newLength - restLenght);
end