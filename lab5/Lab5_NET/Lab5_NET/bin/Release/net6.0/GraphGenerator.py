from os import path
from sys import argv

import matplotlib.pyplot as plt
import networkx as nx


# zwraca ścieżkę pliku obrazu w którym ma zostać wygenerowany obraz
def GetOutputFileName(file):
    return f"{path.splitext(file)[0]}_img.png"


# zwraca linijkę pliku rezultatów z FNF
def GetLine(file):
    fl = open(file, "r")
    line = fl.readlines()[-1]
    line = line.split("= ")[1]
    return line


# przekształca linijkę w graf reprezentowany jako lista
# warstw które mają zostać połączone między sobą
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


# funkcja rysuje graf i zapisuje go do pliku
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
        node_color=[color.get(node, 0.25) for node in G.nodes()],
        with_labels=True,
        cmap=plt.get_cmap("cool"),
    )
    plt.savefig(file, dpi=200)


# # # # # # # # MAIN # # # # # # #

if len(argv) != 2:
    print("Nie podano ścieżki do pliku!")
    exit(1)

inputFile, outputFile = argv[1], GetOutputFileName(argv[1])
Plot(MakeGraph(GetLine(inputFile)), outputFile)
print(f"Graf zapisano do: '{outputFile}'.")
