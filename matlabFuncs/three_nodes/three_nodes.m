clear all; close all;
dt = 1E-06;
t = single(0:dt:0.01);
E = 3.8E8;
D = 1E-3;
ro = 1040;
A = pi * D^2 / 4;
damping =-(1 - 0.001);
FullLength = 0.025;
restLength = FullLength / 2;
k = E * A / restLength;
m = ro * A * restLength;
extension = 0.0075;
load = single(FullLength:extension / length(t):FullLength + extension);
load(end)=[];
a = zeros(1, length(t));
v = zeros(1, length(t));
x = zeros(1, length(t));
f = zeros(1, length(t));
v(1) = 0.6;
x(:) = restLength;
for i = 2:length(t)
    newLength = x(i-1);
	f(i) = f(i) - GetElasticForce(k, restLength, newLength);
    newLength = load(i-1) - x(i-1);
	f(i) = f(i) + GetElasticForce(k, restLength, newLength);
    f(i) = f(i) + ( damping * v(i - 1));
    a(i) = f(i) / m;
    v(i) = v(i - 1) + a(i) * dt;
    x(i) = x(i - 1) + v(i - 1) * dt;
end
figure
%hold on
%legend
tiledlayout(3,1)
% nexttile
% plot(t,load);
% title('load')
nexttile
plot(t,a);
title('a')
nexttile
plot(t,v);
title('v')
nexttile
hold on
plot(t,x);
plot(t,load);
title('x')






















