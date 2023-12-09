﻿using LatinDwarsliggerLogic;

namespace LatinDwarsliggerTest
{
    [TestClass]
    public class HtmlFormatterTests
    {
        [TestMethod]
        public void StripLineNumbers_Test()
        {
            // Arrange
            string[] lines =
                [
                    @"</p><p>",
                    @"Arma virumque canō, Trōiae quī prīmus ab ōrīs<br>",
                    @"Ītaliam, fātō profugus, Lāvīniaque vēnit<br>",
                    @"lītora, multum ille et terrīs iactātus et altō<br>",
                    @"vī superum saevae memorem Iūnōnis ob īram;<br>",
                    @"multa quoque et bellō passus, dum conderet ",
                    @"urbem,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">5</span><br>",
                    @"inferretque deōs Latiō, genus unde Latīnum,<br>",
                    @"Albānīque patrēs, atque altae moenia Rōmae.",
                    @"</p>"
                ];

            string[] expected =
            {
                    @"</p><p>",
                    @"Arma virumque canō, Trōiae quī prīmus ab ōrīs<br>",
                    @"Ītaliam, fātō profugus, Lāvīniaque vēnit<br>",
                    @"lītora, multum ille et terrīs iactātus et altō<br>",
                    @"vī superum saevae memorem Iūnōnis ob īram;<br>",
                    @"multa quoque et bellō passus, dum conderet ",
                    @"urbem,<br>",
                    @"inferretque deōs Latiō, genus unde Latīnum,<br>",
                    @"Albānīque patrēs, atque altae moenia Rōmae.",
                    @"</p>"
            };

            // Act
            string[] actual = HtmlFormatter.StripLineNumbers(lines).ToArray();

            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void MoveParagraphBeginTagsToOwnLine_Test()
        {
            // Arrange
            string[] lines = [@"<p>Hello, World!</p>", @"<p>How are you?</p>"];
            string[] expected = [@"<p>", @"Hello, World!</p>", @"<p>", @"How are you?</p>"];
            // Act
            string[] actual = HtmlFormatter.MoveParagraphBeginTagsToOwnLine(lines).ToArray();
            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void MoveParagraphBeginTagsToOwnLine_MultipleInLine_Test()
        {
            // Arrange
            string[] lines = [@"<p>abc</p><p>def</p>"];
            string[] expected = [@"<p>", @"abc</p>", @"<p>", @"def</p>"];
            // Act
            string[] actual = HtmlFormatter.MoveParagraphBeginTagsToOwnLine(lines).ToArray();
            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void SplitOnBrTags_Test()
        {
            // Arrange
            string[] lines = 
                [
                @"</p><p>",
                @"This is on the first line,",
                @"as is this.<br>",
                @"This, however,",
                @"is the second line<br>",
                @"and this is the third.",
                @"</p>",
                @"<p>This is the fourth.</p>"
                ];
            string[] expected =
                [
                    @"<p>",
                    @"This is on the first line, as is this.",
                    @"This, however, is the second line",
                    @"and this is the third.",
                    @"<p>",
                    @"This is the fourth."
                ];
            // Act
            var actual = HtmlFormatter.SplitOnBrTags(lines).ToArray();
            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void StripTagAttributes_Test()
        {
            // Arrange
            string[] input = [@"<a href=""https://www.thelatinlibrary.com/vergil/aen1.shtml"">Aeneid</a><br><p class=""poem"">Aeneas scopulum interea conscendit</p>"];
            string[] expected = [@"<a>Aeneid</a><br><p>Aeneas scopulum interea conscendit</p>"];
            // Act
            string[] actual = input.StripTagAttributes().ToArray();
            //Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void RemoveRedundantParagraphTags_Test()
        {
            // Arrange
            string[] input =
                [
                    @"<p>",
                    @"<p>",
                    @"Arma virumque canō, Trōiae quī prīmus ab ōrīs",
                    @"Ītaliam, fātō profugus, Lāvīniaque vēnit",
                    @"<p>",
                    @"Mūsa, mihī causās memorā, quō nūmine laesō,",
                    @"quidve dolēns, rēgīna deum tot volvere cāsūs",
                    @"<p>",
                    @"<p>",
                    @"<p>",
                    @"Urbs antīqua fuit, Tyriī tenuēre colōnī,"
                ];

            string[] expected =
                [
                    @"<p>", // fix duplicate
                    @"Arma virumque canō, Trōiae quī prīmus ab ōrīs",
                    @"Ītaliam, fātō profugus, Lāvīniaque vēnit",
                    @"<p>",
                    @"Mūsa, mihī causās memorā, quō nūmine laesō,",
                    @"quidve dolēns, rēgīna deum tot volvere cāsūs",
                    @"<p>", // fix triplicate
                    @"Urbs antīqua fuit, Tyriī tenuēre colōnī,"
                ];

            // Act
            string[] actual = input.RemoveRedundantParagraphTags().ToArray();

            // Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }


        [TestMethod]
        public void FormatAeneid()
        {
            string code = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">
<html><head>
		<title>
			Vergil: Aeneid I
		</title>

		<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
		
		<link rel=""shortcut icon"" href=""http://www.thelatinlibrary.com/icon.ico"">
		
		<link rel=""StyleSheet"" href=""http://www.thelatinlibrary.com/latinlibrary.css"">
		<link rel=""StyleSheet"" media=""print"" href=""http://www.thelatinlibrary.com/latinlibrary_print.css"">
	<link href=""data:text/css,%3Ais(%5Bid*%3D'google_ads_iframe'%5D%2C%5Bid*%3D'taboola-'%5D%2C.taboolaHeight%2C.taboola-placeholder%2C%23credential_picker_container%2C%23credentials-picker-container%2C%23credential_picker_iframe%2C%5Bid*%3D'google-one-tap-iframe'%5D%2C%23google-one-tap-popup-container%2C.google-one-tap-modal-div%2C%23amp_floatingAdDiv%2C%23ez-content-blocker-container)%20%7Bdisplay%3Anone!important%3Bmin-height%3A0!important%3Bheight%3A0!important%3B%7D"" rel=""stylesheet"" type=""text/css""></head>
	
<body>


<h1>P. VERGILI MARONIS AENEIDOS LIBER PRIMVS</h1>


<p class=""internal_navigation"">

</p><p>
Arma virumque canō, Trōiae quī prīmus ab ōrīs<br>
Ītaliam, fātō profugus, Lāvīniaque vēnit<br>
lītora, multum ille et terrīs iactātus et altō<br>
vī superum saevae memorem Iūnōnis ob īram;<br>
multa quoque et bellō passus, dum conderet 
urbem,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">5</span><br>
inferretque deōs Latiō, genus unde Latīnum,<br>
Albānīque patrēs, atque altae moenia Rōmae.
</p>

<p class=""poem"">
Mūsa, mihī causās memorā, quō nūmine laesō,<br>
quidve dolēns, rēgīna deum tot volvere cāsūs<br>
īnsīgnem pietāte virum, tot adīre 
labōrēs&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">10</span><br>
impulerit. Tantaene animīs caelestibus īrae?
</p>

<p class=""poem"">
Urbs antīqua fuit, Tyriī tenuēre colōnī,<br>
Karthāgō, Ītaliam contrā Tiberīnaque longē<br>
ōstia, dīves opum studiīsque asperrima bellī,<br>
quam Iūnō fertur terrīs magis omnibus 
ūnam&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">15</span><br>
posthabitā coluisse Samō; hīc illius arma,<br>
hīc currus fuit; hōc rēgnum dea gentibus esse,<br>
sī quā Fāta sinant, iam tum tenditque fovetque.<br>
Prōgeniem sed enim Trōiānō ā sanguine dūcī<br>
audierat, Tyriās olim quae verteret 
arcēs;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">20</span><br>
hinc populum lātē regem bellōque superbum<br>
ventūrum excidiō Libyae: sīc volvere Parcās.<br>
Id metuēns, veterisque memor Sāturnia bellī,<br>
prīma quod ad Trōiam prō cārīs gesserat Argīs—<br>
necdum etiam causae īrārum saevīque 
dolōrēs&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">25</span><br>
exciderant animō: manet altā mente repostum<br>
iūdicium Paridis sprētaeque iniūria fōrmae,<br>
et genus invīsum, et raptī Ganymēdis honōrēs.<br>
Hīs accēnsa super, iactātōs aequore tōtō<br>
Trōas, rēliquiās Danaum atque immītis 
Achillī,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">30</span><br>
arcēbat longē Latiō, multōsque per annōs<br>
errābant, āctī Fātīs, maria omnia circum.<br>
Tantae mōlis erat Rōmānam condere gentem!
</p>

<p class=""poem"">
Vix e conspectu Siculae telluris in altum<br>
vela dabant laeti, et spumas salis aere 
ruebant,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">35</span><br>
cum Iuno, aeternum servans sub pectore volnus,<br>
haec secum: 'Mene incepto desistere victam,<br>
nec posse Italia Teucrorum avertere regem?<br>
Quippe vetor fatis. Pallasne exurere classem<br>
Argivom atque ipsos potuit submergere 
ponto,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">40</span><br>
unius ob noxam et furias Aiacis Oilei?<br>
Ipsa, Iovis rapidum iaculata e nubibus ignem,<br>
disiecitque rates evertitque aequora ventis,<br>
illum expirantem transfixo pectore flammas<br>
turbine corripuit scopuloque infixit 
acuto.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">45</span><br>
Ast ego, quae divom incedo regina, Iovisque<br>
et soror et coniunx, una cum gente tot annos<br>
bella gero! Et quisquam numen Iunonis adoret<br>
praeterea, aut supplex aris imponet honorem?'
</p>

<p class=""poem"">
Talia flammato secum dea corde 
volutans&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">50</span><br>
nimborum in patriam, loca feta furentibus austris,<br>
Aeoliam venit. Hic vasto rex Aeolus antro<br>
luctantes ventos tempestatesque sonoras<br>
imperio premit ac vinclis et carcere frenat.<br>
Illi indignantes magno cum murmure 
montis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">55</span><br>
circum claustra fremunt; celsa sedet Aeolus arce<br>
sceptra tenens, mollitque animos et temperat iras.<br>
Ni faciat, maria ac terras caelumque profundum<br>
quippe ferant rapidi secum verrantque per auras.<br>
Sed pater omnipotens speluncis abdidit 
atris,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">60</span><br>
hoc metuens, molemque et montis insuper altos<br>
imposuit, regemque dedit, qui foedere certo<br>
et premere et laxas sciret dare iussus habenas.<br>
Ad quem tum Iuno supplex his vocibus usa est:
</p>

<p class=""poem"">
'Aeole, namque tibi divom pater atque hominum 
rex&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">65</span><br>
et mulcere dedit fluctus et tollere vento,<br>
gens inimica mihi Tyrrhenum navigat aequor,<br>
Ilium in Italiam portans victosque Penates:<br>
incute vim ventis submersasque obrue puppes,<br>
aut age diversos et disiice corpora 
ponto.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">70</span><br>
Sunt mihi bis septem praestanti corpore nymphae,<br>
quarum quae forma pulcherrima Deiopea,<br>
conubio iungam stabili propriamque dicabo,<br>
omnis ut tecum meritis pro talibus annos<br>
exigat, et pulchra faciat te prole 
parentem.'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">75</span>
</p>

<p class=""poem"">
Aeolus haec contra: 'Tuus, O regina, quid optes<br>
explorare labor; mihi iussa capessere fas est.<br>
Tu mihi, quodcumque hoc regni, tu sceptra Iovemque<br>
concilias, tu das epulis accumbere divom,<br>
nimborumque facis tempestatumque 
potentem.'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">80</span>
</p>

<p class=""poem"">
Haec ubi dicta, cavum conversa cuspide montem<br>
impulit in latus: ac venti, velut agmine facto,<br>
qua data porta, ruunt et terras turbine perflant.<br>
Incubuere mari, totumque a sedibus imis<br>
una Eurusque Notusque ruunt creberque 
procellis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">85</span><br>
Africus, et vastos volvunt ad litora fluctus.<br>
Insequitur clamorque virum stridorque rudentum.<br>
Eripiunt subito nubes caelumque diemque<br>
Teucrorum ex oculis; ponto nox incubat atra.<br>
Intonuere poli, et crebris micat ignibus 
aether,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">90</span><br>
praesentemque viris intentant omnia mortem.
</p>

<p class=""poem"">
Extemplo Aeneae solvuntur frigore membra:<br>
ingemit, et duplicis tendens ad sidera palmas<br>
talia voce refert: 'O terque quaterque beati,<br>
quis ante ora patrum Troiae sub moenibus 
altis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">95</span><br>
contigit oppetere! O Danaum fortissime gentis<br>
Tydide! Mene Iliacis occumbere campis<br>
non potuisse, tuaque animam hanc effundere dextra,<br>
saevus ubi Aeacidae telo iacet Hector, ubi ingens<br>
Sarpedon, ubi tot Simois correpta sub 
undis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">100</span><br>
scuta virum galeasque et fortia corpora volvit?'
</p>

<p class=""poem"">
Talia iactanti stridens Aquilone procella<br>
velum adversa ferit, fluctusque ad sidera tollit.<br>
Franguntur remi; tum prora avertit, et undis<br>
dat latus; insequitur cumulo praeruptus aquae 
mons.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">105</span><br>
Hi summo in fluctu pendent; his unda dehiscens<br>
terram inter fluctus aperit; furit aestus harenis.<br>
Tris Notus abreptas in saxa latentia torquet—<br>
saxa vocant Itali mediis quae in fluctibus aras—<br>
dorsum immane mari summo; tris Eurus ab 
alto&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">110</span><br>
in brevia et Syrtis urget, miserabile visu,<br>
inliditque vadis atque aggere cingit harenae.<br>
Unam, quae Lycios fidumque vehebat Oronten,<br>
ipsius ante oculos ingens a vertice pontus<br>
in puppim ferit: excutitur pronusque 
magister&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">115</span><br>
volvitur in caput; ast illam ter fluctus ibidem<br>
torquet agens circum, et rapidus vorat aequore vortex.<br>
Adparent rari nantes in gurgite vasto,<br>
arma virum, tabulaeque, et Troia gaza per undas.<br>
Iam validam Ilionei navem, iam fortis 
Achati,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">120</span><br>
et qua vectus Abas, et qua grandaevus Aletes,<br>
vicit hiems; laxis laterum compagibus omnes<br>
accipiunt inimicum imbrem, rimisque fatiscunt.
</p>

<p class=""poem"">
Interea magno misceri murmure pontum,<br>
emissamque hiemem sensit Neptunus, et 
imis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">125</span><br>
stagna refusa vadis, graviter commotus; et alto<br>
prospiciens, summa placidum caput extulit unda.<br>
Disiectam Aeneae, toto videt aequore classem,<br>
fluctibus oppressos Troas caelique ruina,<br>
nec latuere doli fratrem Iunonis et 
irae.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">130</span><br>
Eurum ad se Zephyrumque vocat, dehinc talia fatur:
</p>

<p class=""poem"">
'Tantane vos generis tenuit fiducia vestri?<br>
Iam caelum terramque meo sine numine, venti,<br>
miscere, et tantas audetis tollere moles?<br>
Quos ego—sed motos praestat componere 
fluctus.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">135</span><br>
Post mihi non simili poena commissa luetis.<br>
Maturate fugam, regique haec dicite vestro:<br>
non illi imperium pelagi saevumque tridentem,<br>
sed mihi sorte datum. Tenet ille immania saxa,<br>
vestras, Eure, domos; illa se iactet in 
aula&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">140</span><br>
Aeolus, et clauso ventorum carcere regnet.'
</p>

<p class=""poem"">
Sic ait, et dicto citius tumida aequora placat,<br>
collectasque fugat nubes, solemque reducit.<br>
Cymothoe simul et Triton adnixus acuto<br>
detrudunt navis scopulo; levat ipse 
tridenti;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">145</span><br>
et vastas aperit syrtis, et temperat aequor,<br>
atque rotis summas levibus perlabitur undas.<br>
Ac veluti magno in populo cum saepe coorta est<br>
seditio, saevitque animis ignobile volgus,<br>
iamque faces et saxa volant—furor arma 
ministrat;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">150</span><br>
tum, pietate gravem ac meritis si forte virum quem<br>
conspexere, silent, arrectisque auribus adstant;<br>
ille regit dictis animos, et pectora mulcet,—<br>
sic cunctus pelagi cecidit fragor, aequora postquam<br>
prospiciens genitor caeloque invectus 
aperto&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">155</span><br>
flectit equos, curruque volans dat lora secundo.
</p>

<p class=""poem"">
Defessi Aeneadae, quae proxima litora, cursu<br>
contendunt petere, et Libyae vertuntur ad oras.<br>
Est in secessu longo locus: insula portum<br>
efficit obiectu laterum, quibus omnis ab 
alto&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">160</span><br>
frangitur inque sinus scindit sese unda reductos.<br>
Hinc atque hinc vastae rupes geminique minantur<br>
in caelum scopuli, quorum sub vertice late<br>
aequora tuta silent; tum silvis scaena coruscis<br>
desuper horrentique atrum nemus imminet 
umbra.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">165</span><br>
Fronte sub adversa scopulis pendentibus antrum,<br>
intus aquae dulces vivoque sedilia saxo,<br>
nympharum domus: hic fessas non vincula navis<br>
ulla tenent, unco non alligat ancora morsu.<br>
Huc septem Aeneas collectis navibus 
omni&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">170</span><br>
ex numero subit; ac magno telluris amore<br>
egressi optata potiuntur Troes harena,<br>
et sale tabentis artus in litore ponunt.<br>
Ac primum silici scintillam excudit Achates,<br>
succepitque ignem foliis, atque arida 
circum&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">175</span><br>
nutrimenta dedit, rapuitque in fomite flammam.<br>
Tum Cererem corruptam undis Cerealiaque arma<br>
expediunt fessi rerum, frugesque receptas<br>
et torrere parant flammis et frangere saxo.
</p>

<p class=""poem"">
Aeneas scopulum interea conscendit, et 
omnem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">180</span><br>
prospectum late pelago petit, Anthea si quem<br>
iactatum vento videat Phrygiasque biremis,<br>
aut Capyn, aut celsis in puppibus arma Caici.<br>
Navem in conspectu nullam, tris litore cervos<br>
prospicit errantis; hos tota armenta 
sequuntur&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">185</span><br>
a tergo, et longum per vallis pascitur agmen.<br>
Constitit hic, arcumque manu celerisque sagittas<br>
corripuit, fidus quae tela gerebat Achates;<br>
ductoresque ipsos primum, capita alta ferentis<br>
cornibus arboreis, sternit, tum volgus, et 
omnem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">190</span><br>
miscet agens telis nemora inter frondea turbam;<br>
nec prius absistit, quam septem ingentia victor<br>
corpora fundat humi, et numerum cum navibus aequet.<br>
Hinc portum petit, et socios partitur in omnes.<br>
Vina bonus quae deinde cadis onerarat 
Acestes&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">195</span><br>
litore Trinacrio dederatque abeuntibus heros,<br>
dividit, et dictis maerentia pectora mulcet:
</p>

<p class=""poem"">
'O socii—neque enim ignari sumus ante malorum—<br>
O passi graviora, dabit deus his quoque finem.<br>
Vos et Scyllaeam rabiem penitusque 
sonantis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">200</span><br>
accestis scopulos, vos et Cyclopea saxa<br>
experti: revocate animos, maestumque timorem<br>
mittite: forsan et haec olim meminisse iuvabit.<br>
Per varios casus, per tot discrimina rerum<br>
tendimus in Latium; sedes ubi fata 
quietas&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">205</span><br>
ostendunt; illic fas regna resurgere Troiae.<br>
Durate, et vosmet rebus servate secundis.'
</p>

<p class=""poem"">
Talia voce refert, curisque ingentibus aeger<br>
spem voltu simulat, premit altum corde dolorem.<br>
Illi se praedae accingunt, dapibusque 
futuris;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">210</span><br>
tergora deripiunt costis et viscera nudant;<br>
pars in frusta secant veribusque trementia figunt;<br>
litore aena locant alii, flammasque ministrant.<br>
Tum victu revocant vires, fusique per herbam<br>
implentur veteris Bacchi pinguisque 
ferinae.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">215</span><br>
Postquam exempta fames epulis mensaeque remotae,<br>
amissos longo socios sermone requirunt,<br>
spemque metumque inter dubii, seu vivere credant,<br>
sive extrema pati nec iam exaudire vocatos.<br>
Praecipue pius Aeneas nunc acris 
Oronti,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">220</span><br>
nunc Amyci casum gemit et crudelia secum<br>
fata Lyci, fortemque Gyan, fortemque Cloanthum.
</p>

<p class=""poem"">
Et iam finis erat, cum Iuppiter aethere summo<br>
despiciens mare velivolum terrasque iacentis<br>
litoraque et latos populos, sic vertice 
caeli&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">225</span><br>
constitit, et Libyae defixit lumina regnis.<br>
Atque illum talis iactantem pectore curas<br>
tristior et lacrimis oculos suffusa nitentis<br>
adloquitur Venus: 'O qui res hominumque deumque<br>
aeternis regis imperiis, et fulmine 
terres,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">230</span><br>
quid meus Aeneas in te committere tantum,<br>
quid Troes potuere, quibus, tot funera passis,<br>
cunctus ob Italiam terrarum clauditur orbis?<br>
Certe hinc Romanos olim, volventibus annis,<br>
hinc fore ductores, revocato a sanguine 
Teucri,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">235</span><br>
qui mare, qui terras omni dicione tenerent,<br>
pollicitus, quae te, genitor, sententia vertit?<br>
Hoc equidem occasum Troiae tristisque ruinas<br>
solabar, fatis contraria fata rependens;<br>
nunc eadem fortuna viros tot casibus 
actos&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">240</span><br>
insequitur. Quem das finem, rex magne, laborum?<br>
Antenor potuit, mediis elapsus Achivis,<br>
Illyricos penetrare sinus, atque intima tutus<br>
regna Liburnorum, et fontem superare Timavi,<br>
unde per ora novem vasto cum murmure 
montis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">245</span><br>
it mare proruptum et pelago premit arva sonanti.<br>
Hic tamen ille urbem Patavi sedesque locavit<br>
Teucrorum, et genti nomen dedit, armaque fixit<br>
Troia; nunc placida compostus pace quiescit:<br>
nos, tua progenies, caeli quibus adnuis 
arcem,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">250</span><br>
navibus (infandum!) amissis, unius ob iram<br>
prodimur atque Italis longe disiungimur oris.<br>
Hic pietatis honos? Sic nos in sceptra reponis?'
</p>

<p class=""poem"">
Olli subridens hominum sator atque deorum,<br>
voltu, quo caelum tempestatesque 
serenat,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">255</span><br>
oscula libavit natae, dehinc talia fatur:<br>
'Parce metu, Cytherea: manent immota tuorum<br>
fata tibi; cernes urbem et promissa Lavini<br>
moenia, sublimemque feres ad sidera caeli<br>
magnanimum Aenean; neque me sententia 
vertit.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">260</span><br>
Hic tibi (fabor enim, quando haec te cura remordet,<br>
longius et volvens fatorum arcana movebo)<br>
bellum ingens geret Italia, populosque feroces<br>
contundet, moresque viris et moenia ponet,<br>
tertia dum Latio regnantem viderit 
aestas,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">265</span><br>
ternaque transierint Rutulis hiberna subactis.<br>
At puer Ascanius, cui nunc cognomen Iulo<br>
additur,—Ilus erat, dum res stetit Ilia regno,—<br>
triginta magnos volvendis mensibus orbis<br>
imperio explebit, regnumque ab sede 
Lavini&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">270</span><br>
transferet, et longam multa vi muniet Albam.<br>
Hic iam ter centum totos regnabitur annos<br>
gente sub Hectorea, donec regina sacerdos,<br>
Marte gravis, geminam partu dabit Ilia prolem.<br>
Inde lupae fulvo nutricis tegmine 
laetus&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">275</span><br>
Romulus excipiet gentem, et Mavortia condet<br>
moenia, Romanosque suo de nomine dicet.<br>
His ego nec metas rerum nec tempora pono;<br>
imperium sine fine dedi. Quin aspera Iuno,<br>
quae mare nunc terrasque metu caelumque 
fatigat,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">280</span><br>
consilia in melius referet, mecumque fovebit<br>
Romanos rerum dominos gentemque togatam:<br>
sic placitum. Veniet lustris labentibus aetas,<br>
cum domus Assaraci Phthiam clarasque Mycenas<br>
servitio premet, ac victis dominabitur 
Argis.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">285</span><br>
Nascetur pulchra Troianus origine Caesar,<br>
imperium oceano, famam qui terminet astris,—<br>
Iulius, a magno demissum nomen Iulo.<br>
Hunc tu olim caelo, spoliis Orientis onustum,<br>
accipies secura; vocabitur hic quoque 
votis.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">290</span><br>
Aspera tum positis mitescent saecula bellis;<br>
cana Fides, et Vesta, Remo cum fratre Quirinus,<br>
iura dabunt; dirae ferro et compagibus artis<br>
claudentur Belli portae; Furor impius intus,<br>
saeva sedens super arma, et centum vinctus 
aenis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">295</span><br>
post tergum nodis, fremet horridus ore cruento.'
</p>

<p class=""poem"">
Haec ait, et Maia genitum demittit ab alto,<br>
ut terrae, utque novae pateant Karthaginis arces<br>
hospitio Teucris, ne fati nescia Dido<br>
finibus arceret: volat ille per aera 
magnum&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">300</span><br>
remigio alarum, ac Libyae citus adstitit oris.<br>
Et iam iussa facit, ponuntque ferocia Poeni<br>
corda volente deo; in primis regina quietum<br>
accipit in Teucros animum mentemque benignam.
</p>

<p class=""poem"">
At pius Aeneas, per noctem plurima 
volvens,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">305</span><br>
ut primum lux alma data est, exire locosque<br>
explorare novos, quas vento accesserit oras,<br>
qui teneant, nam inculta videt, hominesne feraene,<br>
quaerere constituit, sociisque exacta referre<br>
Classem in convexo nemorum sub rupe 
cavata&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">310</span><br>
arboribus clausam circum atque horrentibus umbris<br>
occulit; ipse uno graditur comitatus Achate,<br>
bina manu lato crispans hastilia ferro.
</p>

<p class=""poem"">
Cui mater media sese tulit obvia silva,<br>
virginis os habitumque gerens, et virginis 
arma&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">315</span><br>
Spartanae, vel qualis equos Threissa fatigat<br>
Harpalyce, volucremque fuga praevertitur Hebrum.<br>
Namque umeris de more habilem suspenderat arcum<br>
venatrix, dederatque comam diffundere ventis,<br>
nuda genu, nodoque sinus collecta 
fluentis.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">320</span><br>
Ac prior, 'Heus' inquit 'iuvenes, monstrate mearum<br>
vidistis si quam hic errantem forte sororum,<br>
succinctam pharetra et maculosae tegmine lyncis,<br>
aut spumantis apri cursum clamore prementem.'
</p>

<p class=""poem"">
Sic Venus; et Veneris contra sic filius 
orsus:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">325</span><br>
'Nulla tuarum audita mihi neque visa sororum—<br>
O quam te memorem, virgo? Namque haud tibi voltus<br>
mortalis, nec vox hominem sonat: O, dea certe—<br>
an Phoebi soror? an nympharum sanguinis una?—<br>
sis felix, nostrumque leves, quaecumque, 
laborem,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">330</span><br>
et, quo sub caelo tandem, quibus orbis in oris<br>
iactemur, doceas. Ignari hominumque locorumque<br>
erramus, vento huc vastis et fluctibus acti:<br>
multa tibi ante aras nostra cadet hostia dextra.'
</p>

<p class=""poem"">
Tum Venus: 'Haud equidem tali me dignor 
honore;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">335</span><br>
virginibus Tyriis mos est gestare pharetram,<br>
purpureoque alte suras vincire cothurno.<br>
Punica regna vides, Tyrios et Agenoris urbem;<br>
sed fines Libyci, genus intractabile bello.<br>
Imperium Dido Tyria regit urbe 
profecta,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">340</span><br>
germanum fugiens. Longa est iniuria, longae<br>
ambages; sed summa sequar fastigia rerum.
</p>

<p class=""poem"">
'Huic coniunx Sychaeus erat, ditissimus agri<br>
Phoenicum, et magno miserae dilectus amore,<br>
cui pater intactam dederat, primisque 
iugarat&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">345</span><br>
ominibus. Sed regna Tyri germanus habebat<br>
Pygmalion, scelere ante alios immanior omnes.<br>
Quos inter medius venit furor. Ille Sychaeum<br>
impius ante aras, atque auri caecus amore,<br>
clam ferro incautum superat, securus 
amorum&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">350</span><br>
germanae; factumque diu celavit, et aegram,<br>
multa malus simulans, vana spe lusit amantem.<br>
Ipsa sed in somnis inhumati venit imago<br>
coniugis, ora modis attollens pallida miris,<br>
crudeles aras traiectaque pectora 
ferro&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">355</span><br>
nudavit, caecumque domus scelus omne retexit.<br>
Tum celerare fugam patriaque excedere suadet,<br>
auxiliumque viae veteres tellure recludit<br>
thesauros, ignotum argenti pondus et auri.<br>
His commota fugam Dido sociosque 
parabat:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">360</span><br>
conveniunt, quibus aut odium crudele tyranni<br>
aut metus acer erat; navis, quae forte paratae,<br>
corripiunt, onerantque auro: portantur avari<br>
Pygmalionis opes pelago; dux femina facti.<br>
Devenere locos, ubi nunc ingentia 
cernis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">365</span><br>
moenia surgentemque novae Karthaginis arcem,<br>
mercatique solum, facti de nomine Byrsam,<br>
taurino quantum possent circumdare tergo.<br>
Sed vos qui tandem, quibus aut venistis ab oris,<br>
quove tenetis iter? 'Quaerenti talibus 
ille&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">370</span><br>
suspirans, imoque trahens a pectore vocem:
</p>

<p class=""poem"">
'O dea, si prima repetens ab origine pergam,<br>
et vacet annalis nostrorum audire laborum,<br>
ante diem clauso componat Vesper Olympo.<br>
Nos Troia antiqua, si vestras forte per 
auris&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">375</span><br>
Troiae nomen iit, diversa per aequora vectos<br>
forte sua Libycis tempestas adpulit oris.<br>
Sum pius Aeneas, raptos qui ex hoste Penates<br>
classe veho mecum, fama super aethera notus.<br>
Italiam quaero patriam et genus ab Iove 
summo.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">380</span><br>
Bis denis Phrygium conscendi navibus aequor,<br>
matre dea monstrante viam, data fata secutus;<br>
vix septem convolsae undis Euroque supersunt.<br>
Ipse ignotus, egens, Libyae deserta peragro,<br>
Europa atque Asia pulsus.' Nec plura 
querentem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">385</span><br>
passa Venus medio sic interfata dolore est:
</p>

<p class=""poem"">
'Quisquis es, haud, credo, invisus caelestibus auras<br>
vitalis carpis, Tyriam qui adveneris urbem.<br>
Perge modo, atque hinc te reginae ad limina perfer,<br>
Namque tibi reduces socios classemque 
relatam&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">390</span><br>
nuntio, et in tutum versis aquilonibus actam,<br>
ni frustra augurium vani docuere parentes.<br>
Aspice bis senos laetantis agmine cycnos,<br>
aetheria quos lapsa plaga Iovis ales aperto<br>
turbabat caelo; nunc terras ordine 
longo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">395</span><br>
aut capere, aut captas iam despectare videntur:<br>
ut reduces illi ludunt stridentibus alis,<br>
et coetu cinxere polum, cantusque dedere,<br>
haud aliter puppesque tuae pubesque tuorum<br>
aut portum tenet aut pleno subit ostia 
velo.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">400</span><br>
Perge modo, et, qua te ducit via, dirige gressum.'
</p>

<p class=""poem"">
Dixit, et avertens rosea cervice refulsit,<br>
ambrosiaeque comae divinum vertice odorem<br>
spiravere, pedes vestis defluxit ad imos,<br>
et vera incessu patuit dea. Ille ubi 
matrem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">405</span><br>
adgnovit, tali fugientem est voce secutus:<br>
'Quid natum totiens, crudelis tu quoque, falsis<br>
ludis imaginibus? Cur dextrae iungere dextram<br>
non datur, ac veras audire et reddere voces?'
</p>

<p class=""poem"">
Talibus incusat, gressumque ad moenia 
tendit:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">410</span><br>
at Venus obscuro gradientes aere saepsit,<br>
et multo nebulae circum dea fudit amictu,<br>
cernere ne quis eos, neu quis contingere posset,<br>
molirive moram, aut veniendi poscere causas.<br>
Ipsa Paphum sublimis abit, sedesque 
revisit&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">415</span><br>
laeta suas, ubi templum illi, centumque Sabaeo<br>
ture calent arae, sertisque recentibus halant.
</p>

<p class=""poem"">
Corripuere viam interea, qua semita monstrat.<br>
Iamque ascendebant collem, qui plurimus urbi<br>
imminet, adversasque adspectat desuper 
arces.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">420</span><br>
Miratur molem Aeneas, magalia quondam,<br>
miratur portas strepitumque et strata viarum.<br>
Instant ardentes Tyrii pars ducere muros,<br>
molirique arcem et manibus subvolvere saxa,<br>
pars optare locum tecto et concludere 
sulco.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">425</span><br>
[Iura magistratusque legunt sanctumque senatum;]<br>
hic portus alii effodiunt; hic alta theatris<br>
fundamenta locant alii, immanisque columnas<br>
rupibus excidunt, scaenis decora alta futuris.<br>
Qualis apes aestate nova per florea 
rura&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">430</span><br>
exercet sub sole labor, cum gentis adultos<br>
educunt fetus, aut cum liquentia mella<br>
stipant et dulci distendunt nectare cellas,<br>
aut onera accipiunt venientum, aut agmine facto<br>
ignavom fucos pecus a praesepibus 
arcent:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">435</span><br>
fervet opus, redolentque thymo fragrantia mella.<br>
'O fortunati, quorum iam moenia surgunt!'<br>
Aeneas ait, et fastigia suspicit urbis.<br>
Infert se saeptus nebula, mirabile dictu,<br>
per medios, miscetque viris, neque cernitur 
ulli.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">440</span>
</p>

<p class=""poem"">
Lucus in urbe fuit media, laetissimus umbra,<br>
quo primum iactati undis et turbine Poeni<br>
effodere loco signum, quod regia Iuno<br>
monstrarat, caput acris equi; sic nam fore bello<br>
egregiam et facilem victu per saecula 
gentem.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">445</span><br>
Hic templum Iunoni ingens Sidonia Dido<br>
condebat, donis opulentum et numine divae,<br>
aerea cui gradibus surgebant limina, nexaeque<br>
aere trabes, foribus cardo stridebat aenis.<br>
Hoc primum in luco nova res oblata 
timorem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">450</span><br>
leniit, hic primum Aeneas sperare salutem<br>
ausus, et adflictis melius confidere rebus.<br>
Namque sub ingenti lustrat dum singula templo,<br>
reginam opperiens, dum, quae fortuna sit urbi,<br>
artificumque manus inter se operumque 
laborem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">455</span><br>
miratur, videt Iliacas ex ordine pugnas,<br>
bellaque iam fama totum volgata per orbem,<br>
Atridas, Priamumque, et saevum ambobus Achillem.<br>
Constitit, et lacrimans, 'Quis iam locus' inquit 'Achate,<br>
quae regio in terris nostri non plena 
laboris?&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">460</span><br>
En Priamus! Sunt hic etiam sua praemia laudi;<br>
sunt lacrimae rerum et mentem mortalia tangunt.<br>
Solve metus; feret haec aliquam tibi fama salutem.'<br>
Sic ait, atque animum pictura pascit inani,<br>
multa gemens, largoque umectat flumine 
voltum.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">465</span>
</p>

<p class=""poem"">
Namque videbat, uti bellantes Pergama circum<br>
hac fugerent Graii, premeret Troiana iuventus,<br>
hac Phryges, instaret curru cristatus Achilles.<br>
Nec procul hinc Rhesi niveis tentoria velis<br>
adgnoscit lacrimans, primo quae prodita 
somno&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">470</span><br>
Tydides multa vastabat caede cruentus,<br>
ardentisque avertit equos in castra, prius quam<br>
pabula gustassent Troiae Xanthumque bibissent.<br>
Parte alia fugiens amissis Troilus armis,<br>
infelix puer atque impar congressus 
Achilli,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">475</span><br>
fertur equis, curruque haeret resupinus inani,<br>
lora tenens tamen; huic cervixque comaeque trahuntur<br>
per terram, et versa pulvis inscribitur hasta.<br>
Interea ad templum non aequae Palladis ibant<br>
crinibus Iliades passis peplumque 
ferebant,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">480</span><br>
suppliciter tristes et tunsae pectora palmis;<br>
diva solo fixos oculos aversa tenebat.<br>
Ter circum Iliacos raptaverat Hectora muros,<br>
exanimumque auro corpus vendebat Achilles.<br>
Tum vero ingentem gemitum dat pectore ab 
imo,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">485</span><br>
ut spolia, ut currus, utque ipsum corpus amici,<br>
tendentemque manus Priamum conspexit inermis.<br>
Se quoque principibus permixtum adgnovit Achivis,<br>
Eoasque acies et nigri Memnonis arma.<br>
Ducit Amazonidum lunatis agmina peltis&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">490</span><br>
Penthesilea furens, mediisque in milibus ardet,<br>
aurea subnectens exsertae cingula mammae,<br>
bellatrix, audetque viris concurrere virgo.
</p>

<p class=""poem"">
Haec dum Dardanio Aeneae miranda videntur,<br>
dum stupet, obtutuque haeret defixus in 
uno,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">495</span><br>
regina ad templum, forma pulcherrima Dido,<br>
incessit magna iuvenum stipante caterva.<br>
Qualis in Eurotae ripis aut per iuga Cynthi<br>
exercet Diana choros, quam mille secutae<br>
hinc atque hinc glomerantur oreades; illa 
pharetram&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">500</span><br>
fert umero, gradiensque deas supereminet omnis:<br>
Latonae tacitum pertemptant gaudia pectus:<br>
talis erat Dido, talem se laeta ferebat<br>
per medios, instans operi regnisque futuris.<br>
Tum foribus divae, media testudine 
templi,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">505</span><br>
saepta armis, solioque alte subnixa resedit.<br>
Iura dabat legesque viris, operumque laborem<br>
partibus aequabat iustis, aut sorte trahebat:<br>
cum subito Aeneas concursu accedere magno<br>
Anthea Sergestumque videt fortemque 
Cloanthum,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">510</span><br>
Teucrorumque alios, ater quos aequore turbo<br>
dispulerat penitusque alias avexerat oras.<br>
Obstipuit simul ipse simul perculsus Achates<br>
laetitiaque metuque; avidi coniungere dextras<br>
ardebant; sed res animos incognita 
turbat.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">515</span><br>
Dissimulant, et nube cava speculantur amicti,<br>
quae fortuna viris, classem quo litore linquant,<br>
quid veniant; cunctis nam lecti navibus ibant,<br>
orantes veniam, et templum clamore petebant.
</p>

<p class=""poem"">
Postquam introgressi et coram data copia 
fandi,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">520</span><br>
maximus Ilioneus placido sic pectore coepit:<br>
'O Regina, novam cui condere Iuppiter urbem<br>
iustitiaque dedit gentis frenare superbas,<br>
Troes te miseri, ventis maria omnia vecti,<br>
oramus, prohibe infandos a navibus 
ignis,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">525</span><br>
parce pio generi, et propius res aspice nostras.<br>
Non nos aut ferro Libycos populare Penatis<br>
venimus, aut raptas ad litora vertere praedas;<br>
non ea vis animo, nec tanta superbia victis.<br>
Est locus, Hesperiam Grai cognomine 
dicunt,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">530</span><br>
terra antiqua, potens armis atque ubere glaebae;<br>
Oenotri coluere viri; nunc fama minores<br>
Italiam dixisse ducis de nomine gentem.<br>
Hic cursus fuit:<br>
cum subito adsurgens fluctu nimbosus 
Orion&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">535</span><br>
in vada caeca tulit, penitusque procacibus austris<br>
perque undas, superante salo, perque invia saxa<br>
dispulit; huc pauci vestris adnavimus oris.<br>
Quod genus hoc hominum? Quaeve hunc tam barbara morem<br>
permittit patria? Hospitio prohibemur 
harenae;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">540</span><br>
bella cient, primaque vetant consistere terra.<br>
Si genus humanum et mortalia temnitis arma<br>
at sperate deos memores fandi atque nefandi.
</p>

<p class=""poem"">
'Rex erat Aeneas nobis, quo iustior alter,<br>
nec pietate fuit, nec bello maior et 
armis.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">545</span><br>
Quem si fata virum servant, si vescitur aura<br>
aetheria, neque adhuc crudelibus occubat umbris,<br>
non metus; officio nec te certasse priorem<br>
poeniteat. Sunt et Siculis regionibus urbes<br>
armaque, Troianoque a sanguine clarus 
Acestes.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">550</span><br>
Quassatam ventis liceat subducere classem,<br>
et silvis aptare trabes et stringere remos:<br>
si datur Italiam, sociis et rege recepto,<br>
tendere, ut Italiam laeti Latiumque petamus;<br>
sin absumpta salus, et te, pater optime 
Teucrum,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">555</span><br>
pontus habet Libyae, nec spes iam restat Iuli,<br>
at freta Sicaniae saltem sedesque paratas,<br>
unde huc advecti, regemque petamus Acesten.'
</p>

<p class=""poem"">
Talibus Ilioneus; cuncti simul ore fremebant<br>
Dardanidae.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">560</span>
</p>

<p class=""poem"">
Tum breviter Dido, voltum demissa, profatur:<br>
'Solvite corde metum, Teucri, secludite curas.<br>
Res dura et regni novitas me talia cogunt<br>
moliri, et late finis custode tueri.<br>
Quis genus Aeneadum, quis Troiae nesciat 
urbem,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">565</span><br>
virtutesque virosque, aut tanti incendia belli?<br>
Non obtusa adeo gestamus pectora Poeni,<br>
nec tam aversus equos Tyria Sol iungit ab urbe.<br>
Seu vos Hesperiam magnam Saturniaque arva,<br>
sive Erycis finis regemque optatis 
Acesten,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">570</span><br>
auxilio tutos dimittam, opibusque iuvabo.<br>
Voltis et his mecum pariter considere regnis;<br>
urbem quam statuo vestra est, subducite navis;<br>
Tros Tyriusque mihi nullo discrimine agetur.<br>
Atque utinam rex ipse Noto compulsus 
eodem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">575</span><br>
adforet Aeneas! Equidem per litora certos<br>
dimittam et Libyae lustrare extrema iubebo,<br>
si quibus eiectus silvis aut urbibus errat.'
</p>

<p class=""poem"">
His animum arrecti dictis et fortis Achates<br>
et pater Aeneas iamdudum erumpere 
nubem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">580</span><br>
ardebant. Prior Aenean compellat Achates:<br>
'Nate dea, quae nunc animo sententia surgit?<br>
omnia tuta vides, classem sociosque receptos.<br>
Unus abest, medio in fluctu quem vidimus ipsi<br>
submersum; dictis respondent cetera 
matris.'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">585</span>
</p>

<p class=""poem"">
Vix ea fatus erat, cum circumfusa repente<br>
scindit se nubes et in aethera purgat apertum.<br>
Restitit Aeneas claraque in luce refulsit,<br>
os umerosque deo similis; namque ipsa decoram<br>
caesariem nato genetrix lumenque 
iuventae&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">590</span><br>
purpureum et laetos oculis adflarat honores:<br>
quale manus addunt ebori decus, aut ubi flavo<br>
argentum Pariusve lapis circumdatur auro.
</p>

<p class=""poem"">
Tum sic reginam adloquitur, cunctisque repente<br>
improvisus ait: 'Coram, quem quaeritis, 
adsum,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">595</span><br>
Troius Aeneas, Libycis ereptus ab undis.<br>
O sola infandos Troiae miserata labores,<br>
quae nos, reliquias Danaum, terraeque marisque<br>
omnibus exhaustos iam casibus, omnium egenos,<br>
urbe, domo, socias, grates persolvere 
dignas&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">600</span><br>
non opis est nostrae, Dido, nec quicquid ubique est<br>
gentis Dardaniae, magnum quae sparsa per orbem.<br>
Di tibi, si qua pios respectant numina, si quid<br>
usquam iustitia est et mens sibi conscia recti,<br>
praemia digna ferant. Quae te tam laeta 
tulerunt&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">605</span><br>
saecula? Qui tanti talem genuere parentes?<br>
In freta dum fluvii current, dum montibus umbrae<br>
lustrabunt convexa, polus dum sidera pascet,<br>
semper honos nomenque tuum laudesque manebunt,<br>
quae me cumque vocant terrae.' Sic fatus, 
amicum&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">610</span><br>
Ilionea petit dextra, laevaque Serestum,<br>
post alios, fortemque Gyan fortemque Cloanthum.
</p>

<p class=""poem"">
Obstipuit primo aspectu Sidonia Dido,<br>
casu deinde viri tanto, et sic ore locuta est:<br>
'Quis te, nate dea, per tanta pericula 
casus&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">615</span><br>
insequitur? Quae vis immanibus applicat oris?<br>
Tune ille Aeneas, quem Dardanio Anchisae<br>
alma Venus Phrygii genuit Simoentis ad undam?<br>
Atque equidem Teucrum memini Sidona venire<br>
finibus expulsum patriis, nova regna 
petentem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">620</span><br>
auxilio Beli; genitor tum Belus opimam<br>
vastabat Cyprum, et victor dicione tenebat.<br>
Tempore iam ex illo casus mihi cognitus urbis<br>
Troianae nomenque tuum regesque Pelasgi.<br>
Ipse hostis Teucros insigni laude 
ferebat,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">625</span><br>
seque ortum antiqua Teucrorum ab stirpe volebat.<br>
Quare agite, O tectis, iuvenes, succedite nostris.<br>
Me quoque per multos similis fortuna labores<br>
iactatam hac demum voluit consistere terra.<br>
Non ignara mali, miseris succurrere 
disco.'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">630</span>
</p>

<p class=""poem"">
Sic memorat; simul Aenean in regia ducit<br>
tecta, simul divom templis indicit honorem.<br>
Nec minus interea sociis ad litora mittit<br>
viginti tauros, magnorum horrentia centum<br>
terga suum, pinguis centum cum matribus 
agnos,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">635</span><br>
munera laetitiamque dii.
</p>

<p class=""poem"">
At domus interior regali splendida luxu<br>
instruitur, mediisque parant convivia tectis:<br>
arte laboratae vestes ostroque superbo,<br>
ingens argentum mensis, caelataque in 
auro&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">640</span><br>
fortia facta patrum, series longissima rerum<br>
per tot ducta viros antiqua ab origine gentis.
</p>

<p class=""poem"">
Aeneas (neque enim patrius consistere mentem<br>
passus amor) rapidum ad navis praemittit Achaten,<br>
Ascanio ferat haec, ipsumque ad moenia 
ducat;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">645</span><br>
omnis in Ascanio cari stat cura parentis.<br>
Munera praeterea, Iliacis erepta ruinis,<br>
ferre iubet, pallam signis auroque rigentem,<br>
et circumtextum croceo velamen acantho,<br>
ornatus Argivae Helenae, quos illa 
Mycenis,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">650</span><br>
Pergama cum peteret inconcessosque hymenaeos,<br>
extulerat, matris Ledae mirabile donum:<br>
praeterea sceptrum, Ilione quod gesserat olim,<br>
maxima natarum Priami, colloque monile<br>
bacatum, et duplicem gemmis auroque 
coronam.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">655</span><br>
Haec celerans ita ad naves tendebat Achates.
</p>

<p class=""poem"">
At Cytherea novas artes, nova pectore versat<br>
Consilia, ut faciem mutatus et ora Cupido<br>
pro dulci Ascanio veniat, donisque furentem<br>
incendat reginam, atque ossibus implicet 
ignem;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">660</span><br>
quippe domum timet ambiguam Tyriosque bilinguis;<br>
urit atrox Iuno, et sub noctem cura recursat.<br>
Ergo his aligerum dictis adfatur Amorem:
</p>

<p class=""poem"">
'Nate, meae vires, mea magna potentia solus,<br>
nate, patris summi qui tela Typhoia 
temnis,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">665</span><br>
ad te confugio et supplex tua numina posco.<br>
Frater ut Aeneas pelago tuus omnia circum<br>
litora iactetur odiis Iunonis iniquae,<br>
nota tibi, et nostro doluisti saepe dolore.<br>
Hunc Phoenissa tenet Dido blandisque 
moratur&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">670</span><br>
vocibus; et vereor, quo se Iunonia vertant<br>
hospitia; haud tanto cessabit cardine rerum.<br>
Quocirca capere ante dolis et cingere flamma<br>
reginam meditor, ne quo se numine mutet,<br>
sed magno Aeneae mecum teneatur amore.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">675</span><br>
Qua facere id possis, nostram nunc accipe mentem.<br>
Regius accitu cari genitoris ad urbem<br>
Sidoniam puer ire parat, mea maxima cura,<br>
dona ferens, pelago et flammis restantia Troiae:<br>
hunc ego sopitum somno super alta 
Cythera&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">680</span><br>
aut super Idalium sacrata sede recondam,<br>
ne qua scire dolos mediusve occurrere possit.<br>
Tu faciem illius noctem non amplius unam<br>
falle dolo, et notos pueri puer indue voltus,<br>
ut, cum te gremio accipiet laetissima 
Dido&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">685</span><br>
regalis inter mensas laticemque Lyaeum,<br>
cum dabit amplexus atque oscula dulcia figet,<br>
occultum inspires ignem fallasque veneno.'
</p>

<p class=""poem"">
Paret Amor dictis carae genetricis, et alas<br>
exuit, et gressu gaudens incedit Iuli.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">690</span><br>
At Venus Ascanio placidam per membra quietem<br>
inrigat, et fotum gremio dea tollit in altos<br>
Idaliae lucos, ubi mollis amaracus illum<br>
floribus et dulci adspirans complectitur umbra.
</p>

<p class=""poem"">
Iamque ibat dicto parens et dona 
Cupido&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">695</span><br>
regia portabat Tyriis, duce laetus Achate.<br>
Cum venit, aulaeis iam se regina superbis<br>
aurea composuit sponda mediamque locavit.<br>
Iam pater Aeneas et iam Troiana iuventus<br>
conveniunt, stratoque super discumbitur 
ostro.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">700</span><br>
Dant famuli manibus lymphas, Cereremque canistris<br>
expediunt, tonsisque ferunt mantelia villis.<br>
Quinquaginta intus famulae, quibus ordine longam<br>
cura penum struere, et flammis adolere Penatis;<br>
centum aliae totidemque pares aetate 
ministri,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">705</span><br>
qui dapibus mensas onerent et pocula ponant.<br>
Nec non et Tyrii per limina laeta frequentes<br>
convenere, toris iussi discumbere pictis.<br>
Mirantur dona Aeneae, mirantur Iulum<br>
flagrantisque dei voltus simulataque 
verba,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">710</span><br>
[pallamque et pictum croceo velamen acantho.]<br>
Praecipue infelix, pesti devota futurae,<br>
expleri mentem nequit ardescitque tuendo<br>
Phoenissa, et pariter puero donisque movetur.<br>
Ille ubi complexu Aeneae colloque 
pependit&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">715</span><br>
et magnum falsi implevit genitoris amorem,<br>
reginam petit haec oculis, haec pectore toto<br>
haeret et interdum gremio fovet, inscia Dido,<br>
insidat quantus miserae deus; at memor ille<br>
matris Acidaliae paulatim abolere 
Sychaeum&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">720</span><br>
incipit, et vivo temptat praevertere amore<br>
iam pridem resides animos desuetaque corda.
</p>

<p class=""poem"">
Postquam prima quies epulis, mensaeque remotae,<br>
crateras magnos statuunt et vina coronant.<br>
Fit strepitus tectis, vocemque per ampla 
volutant&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">725</span><br>
atria; dependent lychni laquearibus aureis<br>
incensi, et noctem flammis funalia vincunt.<br>
Hic regina gravem gemmis auroque poposcit<br>
implevitque mero pateram, quam Belus et omnes<br>
a Belo soliti; tum facta silentia 
tectis:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">730</span><br>
'Iuppiter, hospitibus nam te dare iura loquuntur,<br>
hunc laetum Tyriisque diem Troiaque profectis<br>
esse velis, nostrosque huius meminisse minores.<br>
Adsit laetitiae Bacchus dator, et bona Iuno;<br>
et vos, O, coetum, Tyrii, celebrate 
faventes.'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">735</span><br>
Dixit, et in mensam laticum libavit honorem,<br>
primaque, libato, summo tenus attigit ore,<br>
tum Bitiae dedit increpitans; ille impiger hausit<br>
spumantem pateram, et pleno se proluit auro<br>
post alii proceres. Cithara crinitus 
Iopas&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">740</span><br>
personat aurata, docuit quem maximus Atlas.<br>
Hic canit errantem lunam solisque labores;<br>
unde hominum genus et pecudes; unde imber et ignes;<br>
Arcturum pluviasque Hyadas geminosque Triones;<br>
quid tantum Oceano properent se tinguere 
soles&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">745</span><br>
hiberni, vel quae tardis mora noctibus obstet.<br>
Ingeminant plausu Tyrii, Troesque sequuntur.<br>
Nec non et vario noctem sermone trahebat<br>
infelix Dido, longumque bibebat amorem,<br>
multa super Priamo rogitans, super Hectore 
multa;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">750</span><br>
nunc quibus Aurorae venisset filius armis,<br>
nunc quales Diomedis equi, nunc quantus Achilles.<br>
'Immo age, et a prima dic, hospes, origine nobis<br>
insidias,' inquit, 'Danaum, casusque tuorum,<br>
erroresque tuos; nam te iam septima 
portat&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=""font-size: 80%;"">755</span><br>
omnibus errantem terris et fluctibus aestas.'
</p>

<div class=""footer"">
	<p>
		<a href=""https://www.thelatinlibrary.com/verg.html"">Vergil</a>
		<a href=""https://www.thelatinlibrary.com/index.html"">The Latin Library</a>
		<a href=""https://www.thelatinlibrary.com/classics.html"">The Classics Page</a>
	</p>
</div>

</body></html>";
            string[] codeLines = code.Split(Environment.NewLine);
            var actual = HtmlFormatter.FormatHtmlCode(codeLines);
            ;
        }
    }
}