close all;
files = dir('*.mat');
figure('NumberTitle', 'off', 'Name','Different models');
hold on;

for file = files'
    %fprintf(1, 'Doing something with %s.\n', file.name)
    %clear('L');
    load(file.name);
    title('Different models')
    xlabel('Counts')
    ylabel('L, m')
    plot(L,'LineWidth',3,'DisplayName',PhysicalModel);
end
legend show