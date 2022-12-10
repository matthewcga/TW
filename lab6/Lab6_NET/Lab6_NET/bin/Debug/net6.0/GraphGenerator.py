from os import path
from sys import argv, stdout

import matplotlib.pyplot as plt
import networkx as nx


# zwraca ścieżkę pliku obrazu w którym ma zostać wygenerowany obraz
def GetOutputFileName(file):
    return f"{path.splitext(file)[0]}_img.png"


# zwraca warstwy FNF z pliku .tmp
def GetFnf(file):
    fl = open(file, "r")
    lines, fnf, chDict = fl.readlines(), [], dict()

    for layer in lines:
        fnfLayer, productions = [], layer.split(";")
        for production in productions:
            if production == "\n":
                continue
            elems = production.split(".")

            v = 1 if not elems[0] in chDict else chDict[elems[0]] + 1
            chDict.update({elems[0]: v})

            if elems[0] == "B":
                fnfLayer.append((elems[0], chDict[elems[0]], elems[1]))
            else:
                fnfLayer.append((elems[0], chDict[elems[0]], elems[1], elems[2]))
        fnf.append(fnfLayer)

    return fnf


def GetNodeName(node):
    name = f"{node[1]}-{node[0]}({node[2]}"
    if len(node) == 4:
        name += f", {node[3]}"
    name += ")"
    return name


# funkcja rysuje graf i zapisuje go do pliku
def Plot(g, file):
    G, color = nx.Graph(), {"S!": 0}

    for curent in range(0, len(g) - 1):
        for i in range(0, len(g[curent])):
            lower = curent + 1
            for j in range(0, len(g[lower])):
                color.update({GetNodeName(g[lower][j]): lower / len(g)})
                G.add_edge(GetNodeName(g[curent][i]), GetNodeName(g[lower][j]))

    nx.draw(
        G,
        node_color=[color.get(node, 0.01) for node in G.nodes()],
        with_labels=True,
        font_size=5,
        cmap=plt.get_cmap("cool"),
    )
    plt.savefig(file, dpi=200)


# # # # # # # # MAIN # # # # # # #

if len(argv) != 3:
    stdout.write("Nie podano ścieżek do plików!")
    exit(1)

inFile, outFile = argv[1], GetOutputFileName(argv[2])
Plot(GetFnf(inFile), outFile)
stdout.write(f"Graf zapisano do: '{outFile}'.")
exit(0)
