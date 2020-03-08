close all;
clear all;
t = [0 0.07	0.14	0.21	0.28	0.35	0.42	0.49	0.56	0.63	0.7	0.77	0.84	0.91	0.98];
dt = 5E-06;
ti = single(0:dt:t(end));
figure('NumberTitle', 'off', 'Name','2 elements');
title('2 elements')
xlabel('t, s')
ylabel('L, m')
load('LhookinterpLoadOriginal3.mat');
LhookinterpLoadOriginal3 = LhookinterpLoadOriginal;
clear('LhookinterpLoadOriginal');
load('LhookGeomNoninterpLoadOriginal3.mat');
LhookGeomNoninterpLoadOriginal3 = LhookGeomNoninterpLoadOriginal;
clear('LhookGeomNoninterpLoadOriginal');
load('LmooneyRivinterpLoadOriginal3.mat');
LmooneyRivinterpLoadOriginal3 = LmooneyRivinterpLoadOriginal;
clear('LmooneyRivinterpLoadOriginal');

hold on;
plot(ti, LhookinterpLoadOriginal3,'LineWidth',3);
plot(ti,LhookGeomNoninterpLoadOriginal3,'LineWidth',3);
plot(ti,LmooneyRivinterpLoadOriginal3,'LineWidth',3);
legend('Linear Hook','Geometry Nonlin Hook',...
    'Mooney Rivlin','Location','northwest');

figure('NumberTitle', 'off', 'Name','8 elements');
title('8 elements')
xlabel('t, s')
ylabel('L, m')
load('LhookinterpLoadOriginal9.mat');
LhookinterpLoadOriginal9 = LhookinterpLoadOriginal;
clear('LhookinterpLoadOriginal');
load('LhookGeomNoninterpLoadOriginal9.mat');
LhookGeomNoninterpLoadOriginal9 = LhookGeomNoninterpLoadOriginal;
clear('LhookGeomNoninterpLoadOriginal');
load('LmooneyRivinterpLoadOriginal9.mat');
LmooneyRivinterpLoadOriginal9 = LmooneyRivinterpLoadOriginal;
clear('LmooneyRivinterpLoadOriginal');

hold on;
plot(ti, LhookinterpLoadOriginal9,'LineWidth',3);
plot(ti,LhookGeomNoninterpLoadOriginal9,'LineWidth',3);
plot(ti,LmooneyRivinterpLoadOriginal9,'LineWidth',3);
legend('Linear Hook','Geometry Nonlin Hook',...
    'Mooney Rivlin','Location','northwest');

figure('NumberTitle', 'off', 'Name','14 elements');
title('14 elements')
xlabel('t, s')
ylabel('L, m')
load('LhookinterpLoadOriginal15.mat');
LhookinterpLoadOriginal15 = LhookinterpLoadOriginal;
clear('LhookinterpLoadOriginal');
load('LhookGeomNoninterpLoadOriginal15.mat');
LhookGeomNoninterpLoadOriginal15 = LhookGeomNoninterpLoadOriginal;
clear('LhookGeomNoninterpLoadOriginal');
load('LmooneyRivinterpLoadOriginal15.mat');
LmooneyRivinterpLoadOriginal15 = LmooneyRivinterpLoadOriginal;
clear('LmooneyRivinterpLoadOriginal');

hold on;
plot(ti, LhookinterpLoadOriginal15,'LineWidth',3);
plot(ti,LhookGeomNoninterpLoadOriginal15,'LineWidth',3);
plot(ti,LmooneyRivinterpLoadOriginal15,'LineWidth',3);
legend('Linear Hook','Geometry Nonlin Hook',...
    'Mooney Rivlin','Location','northwest');

figure('NumberTitle', 'off', 'Name','Compare all Linear Hook');
title('Compare all Linear Hook')
xlabel('t, s')
ylabel('L, m')
hold on;
plot(ti,LhookinterpLoadOriginal3,'LineWidth',3);
plot(ti,LhookinterpLoadOriginal9,'LineWidth',3);
plot(ti,LhookinterpLoadOriginal15,'LineWidth',3);
legend('2 elements','8 elements',...
    '14 elements','Location','northwest');

figure('NumberTitle', 'off', 'Name','Compare all Geometry Nonlin Hook');
title('Compare all Geometry Nonlin Hook')
xlabel('t, s')
ylabel('L, m')
hold on;
plot(ti,LhookGeomNoninterpLoadOriginal3,'LineWidth',3);
plot(ti,LhookGeomNoninterpLoadOriginal9,'LineWidth',3);
plot(ti,LhookGeomNoninterpLoadOriginal15,'LineWidth',3);
legend('2 elements','8 elements',...
    '14 elements','Location','northwest');

figure('NumberTitle', 'off', 'Name','Compare all Mooney-Rivlin');
title('Compare all Mooney-Rivlin')
xlabel('t, s')
ylabel('L, m')
hold on;
plot(ti,LmooneyRivinterpLoadOriginal3,'LineWidth',3);
plot(ti,LmooneyRivinterpLoadOriginal9,'LineWidth',3);
plot(ti,LmooneyRivinterpLoadOriginal15,'LineWidth',3);
legend('2 elements','8 elements',...
    '14 elements','Location','northwest');











