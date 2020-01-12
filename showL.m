close all;
clear all;
load('LhookinterpLoadOriginal.mat')
load('LhookGeomNoninterpLoadOriginal.mat')
load('LmooneyRivinterpLoadOriginal.mat')
t = [0 0.07	0.14	0.21	0.28	0.35	0.42	0.49	0.56	0.63	0.7	0.77	0.84	0.91	0.98];
dt = 5E-06;
ti = single(0:dt:t(end));
hold on;
plot(ti, LhookinterpLoadOriginal)
plot(ti,LhookGeomNoninterpLoadOriginal);
plot(ti,LmooneyRivinterpLoadOriginal);
legend('Linear Hook','Geometry Nonlin Hook', 'Mooney Rivlin','Location','northwest')