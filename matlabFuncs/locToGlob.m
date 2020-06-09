clear all
clc
baseX = 0;
baseY = 0;
baseZ = 0;
ax = -0.00312499981;
ay = 0;
az = 0;

bx = 0;
by = 0.00100000005;
bz = 0;

la = sqrt((ax-baseX)^2 + (ay-baseY)^2 + (az-baseZ)^2);
lb = sqrt((bx-baseX)^2 + (by-baseY)^2 + (bz-baseZ)^2);

cosXa = ax / la;
cosYa = ay / la;
cosZa = az / la;

cosXb = bx / lb;
cosYb = by / lb;
cosZb = bz / lb;

cosXz = (cosYa * cosZb - cosZa * cosYb);
cosYz = -(cosXa * cosZb - cosZa * cosXb);
cosZz = (cosXa * cosYb - cosYa * cosXb);

Tx = 50;
Ty = 50;
Tz = 0;

globX = cosXa * Tx + cosXb * Ty + cosXz * Tz
globY = cosYa * Tx + cosYb * Ty + cosYz * Tz
globZ = cosZa * Tx + cosZb * Ty + cosZz * Tz


% locX = cosXa * globX + cosYa * globY + cosZa * globZ
% locY = cosXb * globX + cosYb * globY + cosZb * globZ
% locZ = cosXz * globX + cosYz * globY + cosZz * globZ













