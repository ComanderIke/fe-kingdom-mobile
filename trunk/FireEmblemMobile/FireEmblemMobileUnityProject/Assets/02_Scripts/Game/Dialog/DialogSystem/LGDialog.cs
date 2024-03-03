using UnityEngine;

namespace Game.Dialog.DialogSystem
{
    public class LGDialog:MonoBehaviour
    {
        [SerializeField]private LGDialogContainerSO dialogContainer;
        [SerializeField]private LGDialogGroupSO dialogGroup;
        [SerializeField]private LGDialogSO dialog;

        [SerializeField]private bool groupedDialogs;
        [SerializeField]private bool startingDialogsOnly;

        [SerializeField]private int selectedDialogGroupIndex;
        [SerializeField]private int selectedDialogIndex;
    }
}