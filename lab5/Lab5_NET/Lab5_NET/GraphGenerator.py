from sys import argv
from os import path
import matplotlib.pyplot as plt
import networkx as nx


def GetOutputFileName(file):
    return f"{path.splitext(file)[0]}_img.png"


def GetLine(file):
    fl = open(file, "r")
    line = fl.readlines()[-1]
    line = line.split("= ")[1]
    return line


def MakeGraph(line):
    line = line.strip()
    line = line.replace("(", "")
    g, i, chDict = [["S!"], []], 1, dict()

    for ch in line:
        if ch == ")":
            g.append([])
            i += 1
        else:
            v = 1 if not ch in chDict else chDict[ch] + 1
            chDict.update({ch: v})
            g[i].append(f"{ch}{chDict[ch]}")
    g.pop()
    return g


def Plot(g, file):
    G, color = nx.Graph(), {"S!": 0}

    for curent in range(0, len(g) - 1):
        for i in range(0, len(g[curent])):
            lower = curent + 1
            for j in range(0, len(g[lower])):
                color.update({g[lower][j]: lower / len(g)})
                G.add_edge(g[curent][i], g[lower][j])

    nx.draw(
        G,
        cmap=plt.get_cmap("jet"),
        node_color=[color.get(node, 0.25) for node in G.nodes()],
        node_size=1000,
    )
    nx.draw_networkx_labels(G, nx.spring_layout(G))
    plt.savefig(file)


# # # # # MAIN # # # # #

if len(argv) != 2:
    print("Nie podano ścieżki do pliku!")
    exit(1)

inputFile, outputFile = argv[1], GetOutputFileName(argv[1])
Plot(MakeGraph(GetLine(inputFile)), outputFile)
print(f"Zapisano graf do : '{outputFile}'.")
