clear all; close all;
t = [0 0.07	0.14	0.21	0.28	0.35	0.42	0.49	0.56	0.63	0.7	0.77	0.84	0.91	0.98];
pmx = [0	0.000185	0.003601	0.004685	0.005782	0.006534	0.007581	0.007585	0.00758	0.006119333	0.002035333	-0.000129333	-0.001618	-0.001792	0];
pmy =[0	0.002124	0.005641	0.006194	0.002261	0.001819	-0.002615	-0.004486	-0.005878	-0.008712667	-0.008942667	-0.008068	-0.007747333	-0.005382	0];
plx =[0.005677	0.009974	0.017444	0.016356	0.016064	0.01711	0.016851	0.016838	0.016838	0.014647	0.008521	0.005274	0.003041	0.00278	0.005677];
ply =[0.011867	0.011533	0.01412	0.014524	0.014139	0.013226	0.010774	0.009585	0.007497	0.003245	0.0029	0.004212	0.004693	0.008241	0.011867];
t_re = [0	0.036956914	0.073553272	0.110329908	0.147286822	0.184424013	0.221020371	0.257977285	0.295114476	0.332612223	0.368667748	0.40652605	0.443482964	0.479538489	0.516495403	0.553452317	0.59040923	0.627366144	0.664323058	0.701279971	0.737335497	0.775193798	0.811249324	0.849107626	0.886064539	0.918064539	0.950064539	0.982064539];
re = [0	-1808.466913	-2959.309494	-3863.542951	-4655.980271	-5652.281134	-6191.533087	-5211.672832	-3827.373613	-2226.058364	-752.9798603	-82.20304151	-509.6588574	-2449.650637	-4248.253185	0	4116.728319	7145.088368	7352.240033	6819.564324	6083.025072	5073.571722	4455.40485	3321.002877	0	0	0	0];
bloodV = [0	-0.184956843	-0.30620633	-0.399095767	-0.480065762	-0.583641595	-0.642416769	-0.537607891	-0.397451706	-0.230168516	-0.071927661	-0.006165228	-0.047266749	-0.246609125	-0.450061652	0	0.468557337	0.819975339	0.844636252	0.78298397	0.696670777	0.581586519	0.509658857	0.384299219	0	0	0	0];
dt = 5E-06;
ti = single(0:dt:t(end));
pmxi = single(makima(t,pmx,ti));
pmyi = single(makima(t,pmy,ti));
plxi = single(makima(t,plx,ti));
plyi = single(makima(t,ply,ti));

pmxi = smoothdata(pmxi,'gaussian','SmoothingFactor',0.1);
pmyi = smoothdata(pmyi,'gaussian','SmoothingFactor',0.1);
plxi = smoothdata(plxi,'gaussian','SmoothingFactor',0.1);
plyi = smoothdata(plyi,'gaussian','SmoothingFactor',0.1);
figure('NumberTitle', 'off', 'Name','Endpoint movement');
title('Endpoint movement')
xlabel('t, s')
ylabel('Coordinates, m')
hold on;
plot(pmxi, pmyi,'LineWidth',3);
plot(plxi,plyi,'LineWidth',3);
legend('Muscle','Leaflet');

L = zeros(length(ti), 1);
for i = 1:length(ti)
    L(i) = sqrt((pmxi(i) - plxi(i))^2 + (pmyi(i) - plyi(i))^2);
end
minLidx = 1;
for i = 2:length(ti)
    if L(minLidx) > L(i)
        minLidx = i;
    end
end
hold on
%plot(ti, L);
%Lsh = circshift(L,length(ti) - minLidx);
%plot(ti, Lsh);

% pmxi = circshift(pmxi,length(ti) - minLidx);
% pmyi = circshift(pmyi,length(ti) - minLidx);
% plxi = circshift(plxi,length(ti) - minLidx);
% plyi = circshift(plyi,length(ti) - minLidx);

rei = single(makima(t_re,re,ti));
bloodVi = single(makima(t_re,bloodV,ti));

[signal, Fs, tm] = rdsamp('mghdb/mgh064');
abp1 = signal(310000:311000,4);
abp = abp1(257:732);
t_abp1 = tm(310000:311000);
t_abp = t_abp1(257:732);
t_abp = t_abp-t_abp(1);
abp = abp * 133.322;
abpi = single(makima(t_abp,abp,ti));
%plot(ti, abpi);
%yyaxis right
%plot(ti,bloodVi);

% tq = single(zeros(3, 1));
% tq(1) = single(0);
% for i = 2:3
% tq(i) = single(tq(i - 1) + dt);
% end
% pmxq = pmxi(1:3);
% pmyq = pmyi(1:3);
% plxq = plxi(1:3);
% plyq = plyi(1:3);
% req = rei(1:3);
% bloodVq = bloodVi(1:3);
% abpq = abpi(1:3);
% save('interpLoadOriginalCrop','tq','pmxq','pmyq','plxq','plyq','req','bloodVq','abpq');

tq = ti;
pmxq = pmxi;
pmyq = pmyi;
plxq = plxi;
plyq = plyi;
req = rei;
bloodVq = bloodVi;
abpq = abpi;
save('interpLoadSmooth','tq','pmxq','pmyq','plxq','plyq','req','bloodVq','abpq');

% tq = single(0:dt:t(end)*2);
% pmxq = [pmxi pmxi];
% pmyq = [pmyi pmyi];
% plxq = [plxi plxi];
% plyq = [plyi plyi];
% req = [rei rei];
% bloodVq = [bloodVi bloodVi];
% abpq = [abpi abpi];
% save('interpLoadOriginal2','tq','pmxq','pmyq','plxq','plyq','req','bloodVq','abpq');