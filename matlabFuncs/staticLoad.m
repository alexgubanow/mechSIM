clear all; close all;
t = [0 0.07	0.14	0.21	0.28	0.35	0.42	0.49	0.56	0.63	0.7	0.77	0.84	0.91	0.98];
x1 = [0	0.000185	0.003601	0.004685	0.005782	0.006534	0.007581	0.007585	0.00758	0.006119333	0.002035333	-0.000129333	-0.001618	-0.001792	0];
y1 =[0	0.002124	0.005641	0.006194	0.002261	0.001819	-0.002615	-0.004486	-0.005878	-0.008712667	-0.008942667	-0.008068	-0.007747333	-0.005382	0];
x2 =[0.005677	0.009974	0.017444	0.016356	0.016064	0.01711	0.016851	0.016838	0.016838	0.014647	0.008521	0.005274	0.003041	0.00278	0.005677];
y2 =[0.011867	0.011533	0.01412	0.014524	0.014139	0.013226	0.010774	0.009585	0.007497	0.003245	0.0029	0.004212	0.004693	0.008241	0.011867];
hold on
plot(t, x2); 
for i = 1:length(x1)
    x2(i) = x2(i)-x1(i);
end
for i = 1:length(y1)
    y2(i) = y2(i)-y1(i);
end
plot(t, x2); 
x1 = zeros(1, length(x1));
y1 = zeros(1, length(y1));

dt = 1E-06;
ti = single(0:dt:t(end));
x1i = single(makima(t,x1,ti));
y1i = single(makima(t,y1,ti));
x2i = single(makima(t,x2,ti));
y2i = single(makima(t,y2,ti));
Lsdv = zeros(length(ti), 1);
for i = 1:length(ti)
    Lsdv(i) = sqrt((x1i(i) - x2i(i))^2 + (y1i(i) - y2i(i))^2);
end
minLidx = 1;
for i = 2:length(ti)
    if Lsdv(minLidx) > Lsdv(i)
        minLidx = i;
    end
end
x1i = circshift(x1i,length(ti) - minLidx);
y1i = circshift(y1i,length(ti) - minLidx);
x2i = circshift(x2i,length(ti) - minLidx);
y2i = circshift(y2i,length(ti) - minLidx);
E = 6E6;
D = 1E-3;
ro = 1040;
A = pi * D^2;
L = zeros(1, length(ti));
Fn = zeros(1, length(ti));
L(1) = sqrt((x2i(1) - x1i(1))^2 + (y2i(1) - y1i(1))^2);
for t = 2:length(ti)
    L(t) = sqrt((x2i(t) - x1i(t))^2 + (y2i(t) - y1i(t))^2);
	Fn(t) = 0 - (E * A / L(1) * ((x1i(t) - x2i(t))-L(1)));
end
figure
plot(Fn);






















