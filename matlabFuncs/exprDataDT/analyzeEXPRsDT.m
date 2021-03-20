close all;
files = dir('*.mat');
figure('NumberTitle', 'off', 'Name','Different time step');
hold on;

for file = files'
    load(file.name);
    title('Different time step')
    xlabel('Counts')
    ylabel('L, m')
    legendSTR = strcat(PhysicalModel, ", ", num2str(dt));
        plot(L,'LineWidth', 2,'DisplayName',legendSTR);
end
legend show













