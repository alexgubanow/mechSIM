clear all
clc

v = [14.2982 -47.9282 0];
k = [19.1802 5.7016 0];

lv = norm(v);%length of vector v
lk = norm(k);

cosx = (v./lv)';
cosy = (k./lk)';

cosz = [
    cosx(2) * cosy(3) - cosx(3) * cosy(2)%cosXz
    -(cosx(1) * cosy(3) - cosx(3) * cosy(1))%cosYz
    cosx(1) * cosy(2) - cosx(2) * cosy(1)%cosZz
    ];

rotM = [cosx cosy cosz];

T = [0 30 0];

t = T.*rotM

t = [28.7563 8.5483 0];

T = t.*rotM'

T= [sum(T(1,:)) sum(T(2,:)) sum(T(3,:))]


% globX = cosXa * Tx + cosXb * Ty + cosXz * Tz
% globY = cosYa * Tx + cosYb * Ty + cosYz * Tz
% globZ = cosZa * Tx + cosZb * Ty + cosZz * Tz


% locX = cosXa * globX + cosYa * globY + cosZa * globZ
% locY = cosXb * globX + cosYb * globY + cosZb * globZ
% locZ = cosXz * globX + cosYz * globY + cosZz * globZ













