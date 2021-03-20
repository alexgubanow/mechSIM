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
    legendSTR = strcat(PhysicalModel, ", ", num2str(nodes));
    plotColorSTR = "";
    if (PhysicalModel == "hook")
        plotColorSTR = "red";
    elseif(PhysicalModel == "hookGeomNon")
        plotColorSTR = "blue";
    elseif(PhysicalModel == "mooneyRiv")
        plotColorSTR = "green";
    end
    plotMarkerSTR ='o';
    if (nodes == 8)
        plotMarkerSTR = 'o';
    elseif(nodes == 16)
        plotMarkerSTR = '*';
    elseif(nodes == 32)
        plotMarkerSTR = '+';
    elseif(nodes == 64)
        plotMarkerSTR = '>';
    end
        plot(L,'LineWidth', 2,'DisplayName',legendSTR, 'LineStyle', ':','Color', plotColorSTR, 'Marker', plotMarkerSTR);
end
legend show













