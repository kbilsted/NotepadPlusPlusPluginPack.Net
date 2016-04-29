import Face
from FileGenerator import Regenerate

def printLexCSFile(f):
	out = []
	for name in f.order:
		v = f.features[name]
		if v["Value"] == "-1": v["Value"] = "0xFFFFFFFF"
		if v["FeatureType"] in ["fun", "get", "set"]:
			if "Comment" in v: out.extend(["        /// " + line for line in v["Comment"]])
			featureDefineName = "SCI_" + name.upper()
			out.append("        " + featureDefineName + " = " + v["Value"] + ",")
			out.append("")
		elif v["FeatureType"] in ["evt"]:
			featureDefineName = "SCN_" + name.upper()
			if "Comment" in v: out.extend(["        /// " + line for line in v["Comment"]])
			out.append("        " + featureDefineName + " = " + v["Value"] + ",")
			out.append("")
		elif v["FeatureType"] in ["val"]:
			if not ("SCE_" in name or "SCLEX_" in name):
				if "Comment" in v: out.extend(["        /// " + line for line in v["Comment"]])
				out.append("        " + name + " = " + v["Value"] + ",")
				out.append("")
	out[-1] = out[-1][:-1] # drop the very last comma of the last line
	return out

def main():
	f = Face.Face()
	f.ReadFromFile("../../../notepad-plus-plus/scintilla/include/Scintilla.iface")
	Regenerate("../../Visual Studio Project Template C#/Integration/Scintilla_iface.cs", "/* ", printLexCSFile(f))

if __name__ == "__main__":
	main()
