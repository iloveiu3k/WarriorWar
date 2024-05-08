using System.Collections.Generic;

public class SelectionManager
{
    private static SelectionManager _instance;
    public static SelectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SelectionManager();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    public HashSet<SelectTableUnit> SelectedUnits = new HashSet<SelectTableUnit>();
    public List<SelectTableUnit> AvailableUnits = new List<SelectTableUnit>();
    public HashSet<SelectTableUnit> EnemySelectedUnits = new HashSet<SelectTableUnit>();
    public List<SelectTableUnit> EnemyAvailableUnits = new List<SelectTableUnit>();

    public void Select(SelectTableUnit Unit)
    {
        SelectedUnits.Add(Unit);
        Unit.OnSelected();
    }
    public void DeSelect(SelectTableUnit Unit)
    {
        Unit.OnDeSelected();
        SelectedUnits.Remove(Unit);
    }
    public void SelectEnemy(SelectTableUnit Enemy)
    {
        EnemySelectedUnits.Add(Enemy);
        Enemy.OnSelected();

    }
    public void DeSelectEnemy(SelectTableUnit Enemy)
    {
        Enemy.OnDeSelected();
        EnemySelectedUnits.Remove(Enemy);
    }
    public void DeSelectAll()
    {
        foreach(SelectTableUnit unit in SelectedUnits)
        {
            unit.OnDeSelected();
        }
        SelectedUnits.Clear();
    }
    public void DeSelectAllEnemy()
    {
        foreach (SelectTableUnit enemy in EnemySelectedUnits)
        {
            if (enemy != null)
            {
                enemy.OnDeSelected();
            }
        }
        EnemySelectedUnits.Clear();
    }
    public bool IsSelectedEnemy(SelectTableUnit Enemy)
    {
        return EnemySelectedUnits.Contains(Enemy);
    }
    public bool IsSelected(SelectTableUnit Unit)
    {
        return SelectedUnits.Contains(Unit);
    }
    public void DeUnit(SelectTableUnit deUnit)
    {
        AvailableUnits.Remove(deUnit);
    }public void DeEnemyUnit(SelectTableUnit deUnit)
    {
        EnemyAvailableUnits.Remove(deUnit);
    }
}
